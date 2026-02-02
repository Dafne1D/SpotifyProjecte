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
    public static List<Song> GetByPlaylistId(SpotifyDBConnection dbConn, Guid playlistId)
    {
        List<Song> songs = new();

        dbConn.Open();
        string sql = @"
            SELECT s.Id, s.Title, s.Artist, s.Album, s.Duration, s.Genre, s.ImageUrl
            FROM Songs s
            INNER JOIN PlaylistSongs ps ON s.Id = ps.SongId
            WHERE ps.PlaylistId = @PlaylistId";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@PlaylistId", playlistId);
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