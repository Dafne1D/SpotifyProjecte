using Microsoft.Data.SqlClient;
using SpotifyAPI.Model;
using SpotifyAPI.Services;

namespace SpotifyAPI.Repository;

static class SongADO
{
    public static void Insert(SpotifyDBConnection dbConn, Song song)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Songs (Id, Title, Artist, Album, Duration, Genre, ImageUrl)
                    VALUES (@Id, @Title, @Artist, @Album, @Duration, @Genre, @ImageUrl)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", song.Id);
        cmd.Parameters.AddWithValue("@Title", song.Title);
        cmd.Parameters.AddWithValue("@Artist", song.Artist);
        cmd.Parameters.AddWithValue("@Album", song.Album);
        cmd.Parameters.AddWithValue("@Duration", song.Duration);
        cmd.Parameters.AddWithValue("@Genre", song.Genre);
        cmd.Parameters.AddWithValue("@ImageUrl", song.ImageUrl);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

    public static List<Song> GetAll(SpotifyDBConnection dbConn)
    {
        List<Song> songs = new();

        dbConn.Open();
        string sql = "SELECT Id, Title, Artist, Album, Duration, Genre, ImageUrl FROM Songs";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            songs.Add(new Song
            {
                Id = reader.GetGuid(0),
                Title = reader.GetString(1),
                Artist = reader.GetString(2),
                Album = reader.GetString(3),
                Duration = reader.GetInt32(4),
                Genre = reader.GetString(5),
                ImageUrl = reader.GetString(6)
            });
        }

        dbConn.Close();
        return songs;
    }

    public static Song? GetById(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Title, Artist, Album, Duration, Genre, ImageUrl FROM Songs WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Song? song = null;

        if (reader.Read())
        {
            song = new Song
            {
                Id = reader.GetGuid(0),
                Title = reader.GetString(1),
                Artist = reader.GetString(2),
                Album = reader.GetString(3),
                Duration = reader.GetInt32(4),
                Genre = reader.GetString(5),
                ImageUrl = reader.GetString(6)
            };
        }

        dbConn.Close();
        return song;
    }

    public static void Update(SpotifyDBConnection dbConn, Song song)
    {
        dbConn.Open();

        string sql = @"UPDATE Songs SET
                    Title = @Title,
                    Artist = @Artist,
                    Album = @Album,
                    Duration = @Duration,
                    Genre = @Genre,
                    ImageUrl = @ImageUrl
                    WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", song.Id);
        cmd.Parameters.AddWithValue("@Title", song.Title);
        cmd.Parameters.AddWithValue("@Artist", song.Artist);
        cmd.Parameters.AddWithValue("@Album", song.Album);
        cmd.Parameters.AddWithValue("@Duration", song.Duration);
        cmd.Parameters.AddWithValue("@Genre", song.Genre);
        cmd.Parameters.AddWithValue("@ImageUrl", song.ImageUrl);

        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine($"{rows} files actualitzades");

        dbConn.Close();
    }

    public static bool Delete(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Songs WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}