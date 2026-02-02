using Microsoft.Data.SqlClient;
using SpotifyAPI.Model;
using SpotifyAPI.Services;

namespace SpotifyAPI.Repository;

static class SongFileADO
{
    public static void Insert(SpotifyDBConnection dbConn, SongFile songFile)
    {
        dbConn.Open();

        string sql = """
            INSERT INTO SongFiles (Id, SongId, Url)
            VALUES (@Id, @SongId, @Url)
            """;

        using SqlCommand cmd = new(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", songFile.Id);
        cmd.Parameters.AddWithValue("@SongId", songFile.SongId);
        cmd.Parameters.AddWithValue("@Url", songFile.Url);

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static List<SongFile> GetAllFiles(SpotifyDBConnection dbConn)
{
    dbConn.Open();

    string sql = "SELECT Id, SongId, Url FROM SongFiles";

    using SqlCommand cmd = new(sql, dbConn.sqlConnection);
    using SqlDataReader reader = cmd.ExecuteReader();

    List<SongFile> list = new();

    while (reader.Read())
    {
        list.Add(new SongFile
        {
            Id = reader.GetGuid(0),
            SongId = reader.GetGuid(1),
            Url = reader.GetString(2)
        });
    }

    dbConn.Close();
    return list;
}


    public static SongFile? GetById(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = "SELECT Id, SongId, UrlFROM, SongFiles WHERE Id = @Id";

        using SqlCommand cmd = new(sql, dbConn.sqlConnection);
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

    public static List<SongFile> GetAllBySongId(SpotifyDBConnection dbConn, Guid songId)
    {
        dbConn.Open();

        string sql = "SELECT Id, SongId, UrlFROM SongFilesWHERE SongId = @SongId";

        using SqlCommand cmd = new(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@SongId", songId);

        using SqlDataReader reader = cmd.ExecuteReader();

        List<SongFile> list = new();

        while (reader.Read())
        {
            list.Add(new SongFile
            {
                Id = reader.GetGuid(0),
                SongId = reader.GetGuid(1),
                Url = reader.GetString(2)
            });
        }

        dbConn.Close();
        return list;
    }

    public static bool Delete(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = "DELETE FROM SongFiles WHERE Id = @Id";

        using SqlCommand cmd = new(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();
        dbConn.Close();

        return rows > 0;
    }
}
