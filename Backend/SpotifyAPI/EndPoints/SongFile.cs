using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.Services;

namespace SpotifyAPI.EndPoints;

public static class SongFileEndpoints
{
    public static void MapSongFileEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /songfiles
        app.MapPost("/songFiles", (SongFileRequest req) =>
        {
            SongFile songFile = new SongFile
            {
                Id = Guid.NewGuid(),
                SongId = req.SongId,
                Url = req.Url
            };

            SongFileADO.Insert(dbConn, songFile);

            return Results.Created($"/songFiles/{songFile.Id}", songFile);
        });
    }
}

public record SongFileRequest(Guid SongId, string Url);