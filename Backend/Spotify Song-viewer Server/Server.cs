using Spotify_Song_viewer_Server.Models;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;

class Server
{
    private const int Port = 6000;
    private static TcpListener listener;

    private static readonly Dictionary<TcpClient, ClientInfo> clients = new();
    private static readonly object clientsLock = new();

    private static readonly HttpClient httpClient = new HttpClient();

    static async Task Main()
    {
        listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        Console.WriteLine($"Server listening on port {Port}");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _ = HandleClient(client);
        }
    }

    private static async Task HandleClient(TcpClient client)
    {
        Console.WriteLine("Client connected");

        try
        {
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new(stream);
            using StreamWriter writer = new(stream) { AutoFlush = true };

            bool registered = false;

            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null)
                    break; // disconnect

                using JsonDocument doc = JsonDocument.Parse(line);
                string type = doc.RootElement.GetProperty("type").GetString()!;

                if (!registered)
                {
                    if (type != "Join")
                        continue;

                    Guid userId = doc.RootElement.GetProperty("userId").GetGuid()!;
                    Guid songId = doc.RootElement.GetProperty("songId").GetGuid()!;

                    string userApi = await GetUser(userId);
                    string songApi = await GetSong(songId);

                    Console.WriteLine("USER: "+userApi);
                    Console.WriteLine("SONG: "+songApi);

                    User user = new User(userId, "username", "");
                    Song song = new Song(songId, "songName", "Unknown", "Unknown", 0, "Unknown", "");

                    lock (clientsLock)
                    {
                        clients[client] = new ClientInfo(user, song);
                    }

                    registered = true;
                    BroadcastUserList();
                }
                else
                {
                    if (type == "UpdateSong")
                    {
                        Guid newSongId = doc.RootElement.GetProperty("songId").GetGuid()!;
                        Song song = new Song(newSongId, "newSong", "Unknown", "Unknown", 0, "Unknown", "");

                        lock (clientsLock)
                        {
                            clients[client].Song = song;
                        }

                        BroadcastUserList();
                    }
                }
            }
        }
        catch (Exception)
        {
            // treat as disconnect
        }
        finally
        {
            lock (clientsLock)
            {
                clients.Remove(client);
            }

            client.Close();
            BroadcastUserList();
            Console.WriteLine("Client disconnected");
        }
    }

    private static Task<string?> GetUser(Guid userId)
    {
        string url = $"http://localhost:5000/users/{userId}";
        return GetFromApiAsync(url);
    }

    private static Task<string?> GetSong(Guid songId)
    {
        string url = $"http://localhost:5000/songs/{songId}";
        return GetFromApiAsync(url);
    }

    private static async Task<string?> GetFromApiAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{url}?requesterId=99999999-9999-9999-9999-999999999999");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
            return null;
        }
    }

    private static void BroadcastUserList()
    {
        List<object> snapshot;

        lock (clientsLock)
        {
            snapshot = clients.Values
                .Select(c => new { username = c.User, song = c.Song })
                .Cast<object>()
                .ToList();
        }

        var message = JsonSerializer.Serialize(new
        {
            type = "UserList",
            users = snapshot
        });

        lock (clientsLock)
        {
            foreach (var kv in clients)
            {
                try
                {
                    NetworkStream stream = kv.Key.GetStream();
                    StreamWriter writer = new(stream) { AutoFlush = true };
                    writer.WriteLine(message);
                }
                catch
                {
                    // ignore broken sockets
                }
            }
        }
    }
}
