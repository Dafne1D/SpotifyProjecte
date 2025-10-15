using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;

namespace SpotifyAPI.EndPoints;

public static class SongEndpoints
{
    public static void MapSongEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /songs
        app.MapPost("/songs", (SongRequest req) =>
        {
            Song song = new Song
            {
                Id = Guid.NewGuid(),
                Title = req.Title,
                Artist = req.Artist,
                Album = req.Album,
                Duration = req.Duration,
                Genre = req.Genre,
                ImageUrl = req.ImageUrl
            };

            SongADO.Insert(dbConn, song);

            return Results.Created($"/songs/{song.Id}", song);
        });

        // GET /songs
        app.MapGet("/songs", () =>
        {
            List<Song> songs = SongADO.GetAll(dbConn);
            return Results.Ok(songs);
        });

        // GET /songs Song by id
        app.MapGet("/songs/{id}", (Guid id) =>
        {
            Song? song = SongADO.GetById(dbConn, id);

            return song is not null
                ? Results.Ok(song)
                : Results.NotFound(new { message = $"Song with Id {id} not found." });
        });

        // PUT /songs by id
        app.MapPut("/songs/{id}", (Guid id, SongRequest req) =>
        {
            Song? existing = SongADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Song updated = new Song
            {
                Id = id,
                Title = req.Title,
                Artist = req.Artist,
                Album = req.Album,
                Duration = req.Duration,
                Genre = req.Genre,
                ImageUrl = req.ImageUrl
            };

            SongADO.Update(dbConn, updated);

            return Results.Ok(updated);
        });

        // DELETE /songs by id
        app.MapDelete("/songs/{id}", (Guid id) => SongADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());
    }
}

public record SongRequest(string Title, string Artist, string Album, int Duration, string Genre, string ImageUrl);