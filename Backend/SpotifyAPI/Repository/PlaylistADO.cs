using Microsoft.Data.SqlClient;
using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Utils;

namespace SpotifyAPI.Repository;
static class PlaylistADO
{
    public static void Insert(SpotifyDBConnection dbConn, Playlist playlist)
    {

        dbConn.Open();

        string sql = @"INSERT INTO Playlists (Id, UserId, Name, Description)
                      VALUES (@Id, @UserId, @Name, @Description)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", playlist.Id);
        cmd.Parameters.AddWithValue("@UserId", playlist.UserId);
        cmd.Parameters.AddWithValue("@Name", playlist.Name);
        cmd.Parameters.AddWithValue("@Description", playlist.Description);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

    public static List<Playlist> GetAll(SpotifyDBConnection dbConn)
    {
        List<Playlist> playlists = new();

        dbConn.Open();
        string sql = "SELECT Id, UserId, Name, Description FROM Playlists";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            playlists.Add(new Playlist
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                Name = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3)
            });
        }

        dbConn.Close();
        return playlists;
    }

}