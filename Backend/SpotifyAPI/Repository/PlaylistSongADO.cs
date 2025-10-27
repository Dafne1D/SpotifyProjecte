using Microsoft.Data.SqlClient;
using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Utils;

namespace SpotifyAPI.Repository;

static class PlaylistSongADO
{
    public static void Insert(SpotifyDBConnection dbConn, PlaylistSong playlistsong)
    {

        dbConn.Open();

        string sql = @"INSERT INTO PlaylistSongs (Id, PlaylistId, SongId)
                      VALUES (@Id, @PlaylistId, @SongId)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", playlistsong.Id);
        cmd.Parameters.AddWithValue("@PlaylistId", playlistsong.PlaylistId);
        cmd.Parameters.AddWithValue("@SongId", playlistsong.SongId);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

    public static bool Delete(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM PlaylistSongs WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}