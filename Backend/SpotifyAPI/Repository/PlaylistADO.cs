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

}