using System.Net.Http.Headers;
using Microsoft.Data.SqlClient;
using SpotifyAPI.Model;
using SpotifyAPI.Services;

namespace SpotifyAPI.Repository;

static class SongFileADO
{
    public static void Insert(SpotifyDBConnection dbConn, SongFile songFile)
    {
        dbConn.Open();

        string sql = @"INSERT INTO SongFiles (Id, SongId, Url)
                    VALUES (@Id, @SongId, @Url)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", songFile.Id);
        cmd.Parameters.AddWithValue("@SongId", songFile.SongId);
        cmd.Parameters.AddWithValue("@Url", songFile.Url);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

    public static SongFile? GetById(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, SongId, Url FROM SongFiles WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        SongFile? songFile = null;

        if (reader.Read())
        {
            songFile = new SongFile
            {
                Id = reader.GetGuid(0),
                SongId = reader.GetGuid(1),
                Url = reader.GetString(2)
            };
        }

        dbConn.Close();
        return songFile;
    }

    public static bool Delete(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM SongFiles WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}

/*CREATE TABLE SongFiles (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    SongId UNIQUEIDENTIFIER NOT NULL,
    Url NVARCHAR(255) NOT NULL,
    CONSTRAINT FKSongsFiles FOREIGN KEY (SongId)
        REFERENCES Songs(Id)
);*/