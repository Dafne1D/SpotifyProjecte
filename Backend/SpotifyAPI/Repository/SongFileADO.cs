using Microsoft.Data.SqlClient;
using SpotifyAPI.Model;
using SpotifyAPI.Services;

static class SongFileADO
{
    public static void Insert(SpotifyDBConnection dbConn, SongFile songFile)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Songs (Id, SongId, Url)
                    VALUES (@Id, @SongId, @Url)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", songFile.Id);
        cmd.Parameters.AddWithValue("@SongId", songFile.SongId);
        cmd.Parameters.AddWithValue("@Url", songFile.Url);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }
}

/*CREATE TABLE SongFiles (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    SongId UNIQUEIDENTIFIER NOT NULL,
    Url NVARCHAR(255) NOT NULL,
    CONSTRAINT FKSongsFiles FOREIGN KEY (SongId)
        REFERENCES Songs(Id)
);*/