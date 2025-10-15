using Microsoft.Data.SqlClient;
using SpotifyAPI.Model;
using SpotifyAPI.Services;

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
}

/*CREATE TABLE Songs (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Artist NVARCHAR(100) NOT NULL,
    Album NVARCHAR(100),
    Duration INT,
    Genre NVARCHAR(50),
    ImageUrl NVARCHAR(255)
);*/