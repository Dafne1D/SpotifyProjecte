using System.Net;
using System.Net.Sockets;
using System.Text.Json;

using Spotify_Song_viewer_Server.Models;

class Server
{
    private const int Port = 5000;
    private static TcpListener listener;

    private static readonly Dictionary<TcpClient, ClientInfo> clients = new();
    private static readonly object clientsLock = new();

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

                    string username = doc.RootElement.GetProperty("username").GetString()!;
                    string songName = doc.RootElement.GetProperty("song").GetString()!;

                    User user = new User(Guid.NewGuid(), username, "");
                    Song song = new Song(Guid.NewGuid(), songName, "Unknown", "Unknown", 0, "Unknown", "");

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
                        string newSong = doc.RootElement.GetProperty("song").GetString()!;
                        Song song = new Song(Guid.NewGuid(), newSong, "Unknown", "Unknown", 0, "Unknown", "");

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
