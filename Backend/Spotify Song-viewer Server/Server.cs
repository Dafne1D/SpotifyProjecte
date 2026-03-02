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
                    {
                        SendError(writer, $"Expected message of type Join (recieved {type}). Disconnecting...");
                        break;
                    }

                    Guid userId = doc.RootElement.GetProperty("userId").GetGuid()!;
                    Guid songId = doc.RootElement.GetProperty("songId").GetGuid()!;

                    User user = await GetUser(userId);
                    if (user == null)
                    {
                        SendError(writer, "Error on User ID. Disconnecting...");
                        break;
                    }
                    Song song = await GetSong(songId);
                    if (song == null)
                    {
                        SendError(writer, "Error on Song ID. Disconnecting...");
                        break;
                    }

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
                        Song newSong = await GetSong(newSongId);
                        if ( newSong == null )
                        {
                            SendError(writer, "Error on New Song ID");
                            continue;
                        }

                        lock (clientsLock)
                        {
                            clients[client].Song = newSong;
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

    private static async Task<User?> GetUser(Guid userId)
    {
        string url = $"http://localhost:5000/users/{userId}";

        string? json = await GetFromApiAsync(url);
        if (json == null)
            return null;

        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        Guid id = root.GetProperty("id").GetGuid();
        string? username = root.GetProperty("username").GetString();
        string? email = root.GetProperty("email").GetString();

        return new User(id, username, email);
    }

    private static async Task<Song?> GetSong(Guid songId)
    {
        string url = $"http://localhost:5000/songs/{songId}";
        string? json = await GetFromApiAsync(url);
        if (json == null)
            return null;

        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;

        Guid id = root.GetProperty("id").GetGuid();
        string? title = root.GetProperty("title").GetString();
        string? artist = root.GetProperty("artist").GetString();
        string? album = root.GetProperty("album").GetString();
        int duration = root.GetProperty("duration").GetInt16();
        string? genre = root.GetProperty("genre").GetString();
        string? imageUrl = root.GetProperty("imageUrl").GetString();

        return new Song(id, title, artist, album, duration, genre, imageUrl);
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

    private static void SendError(StreamWriter writer, string message)
    {
        var errorMessage = JsonSerializer.Serialize(new
        {
            type = "Error",
            message = message
        });

        writer.WriteLine(errorMessage);
        Console.WriteLine($"Client error: {message}");
    }

    private static void BroadcastUserList()
    {
        List<object> snapshot;

        lock (clientsLock)
        {
            snapshot = clients.Values
                .Select(c => new { user = c.User, song = c.Song })
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
