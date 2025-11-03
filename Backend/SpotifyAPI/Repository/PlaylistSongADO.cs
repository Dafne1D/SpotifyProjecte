using Microsoft.Data.SqlClient;

using SpotifyAPI.Services;
using SpotifyAPI.Model;

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

    public static bool Delete(SpotifyDBConnection dbConn, Guid playlistId, Guid songId)
    {
        dbConn.Open();

        string sql = @"DELETE FROM PlaylistSongs
                            WHERE PlaylistId = @PlaylistId
                            AND SongId = @SongId";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@PlaylistId", playlistId);
        cmd.Parameters.AddWithValue("@SongId", songId);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}