using System.Net.Sockets;
using System.Text.Json;

class TestClient
{
    private const int Port = 5000;
    private const string ServerIp = "127.0.0.1";

    static async Task Main()
    {
        using TcpClient client = new TcpClient();
        await client.ConnectAsync(ServerIp, Port);

        Console.WriteLine("Connected to server");

        NetworkStream stream = client.GetStream();
        StreamReader reader = new(stream);
        StreamWriter writer = new(stream) { AutoFlush = true };

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

        _ = Task.Run(async () =>
        {
            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null)
                    break;

                Console.WriteLine("\n--- Server update ---");
                Console.WriteLine(line);
                Console.WriteLine("---------------------");
            }
        });

        while (true)
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

            writer.WriteLine(JsonSerializer.Serialize(updateMessage));
        }

        client.Close();
        Console.WriteLine("Disconnected");
    }
}
