using System.Net.Sockets;
using System.Text.Json;

class TestClient
{
    private const int Port = 6000;
    private const string ServerIp = "127.0.0.1";

    static async Task Main()
    {
        using TcpClient client = new TcpClient();
        await client.ConnectAsync(ServerIp, Port);

        NetworkStream stream = client.GetStream();
        StreamReader reader = new(stream);
        StreamWriter writer = new(stream) { AutoFlush = true };

        bool connected;

        Console.WriteLine("Connected to server");

        Console.Write("User ID: ");
        string userId = Console.ReadLine()!;

        Console.Write("Initial song ID: ");
        string songId = Console.ReadLine()!;

        var joinMessage = new
        {
            type = "Join",
            userId = userId,
            songId = songId
        };

        writer.WriteLine(JsonSerializer.Serialize(joinMessage));

        connected = true;

        _ = Task.Run(async () =>
        {
            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null)
                {
                    connected = false;
                    break;
                }

                using JsonDocument doc = JsonDocument.Parse(line);
                string type = doc.RootElement.GetProperty("type").GetString()!;

                if (type.ToLower() == "error")
                {
                    string message = doc.RootElement.GetProperty("message").GetString()!;
                    Console.Clear();
                    Console.WriteLine($"\nERROR: {message}");
                }
                else if (type.ToLower() == "userlist")
                {
                    JsonElement usersElement = doc.RootElement.GetProperty("users");

                    Console.Clear();
                    Console.WriteLine($"\nUSER LIST: {usersElement.GetRawText()}");

                    if (usersElement.GetArrayLength() == 0)
                    {
                        Console.WriteLine("No users connected.");
                    }

                    foreach (JsonElement item in usersElement.EnumerateArray())
                    {
                        JsonElement user = item.GetProperty("user");
                        JsonElement song = item.GetProperty("song");

                        string username = user.GetProperty("Username").GetString();
                        string songTitle = song.GetProperty("Title").GetString();

                        Console.WriteLine($"- {username} is listening to {songTitle}");
                    }
                    Console.Write("\nNew song ID (empty to quit): ");
                }
                else
                {
                    Console.WriteLine("\nSERVER: " + line);
                }
            }
        });

        while (connected)
        {
            Console.Write("\nNew song ID (empty to quit): ");
            string? newSongId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newSongId))
                break;

            var updateMessage = new
            {
                type = "UpdateSong",
                songId = newSongId
            };

            try
            {
                writer.WriteLine(JsonSerializer.Serialize(updateMessage));
            }
            catch
            {
                Console.WriteLine("Remote disconnect");
                connected = false;
            }
        }

        client.Close();
        Console.WriteLine("Disconnected");
    }
}
