using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;

namespace SpotifyAPI.EndPoints;

public static class PlaylistEndpoints
{

    public static void MapPlaylistEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /playlists
        app.MapPost("/playlists", (PlaylistRequest req) =>
        {
            Playlist playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                UserId = req.UserId,
                Name = req.Name,
                Description = req.Description
            };
            PlaylistADO.Insert(dbConn, playlist);
            return Results.Created($"/playlists/{playlist.Id}", playlist);
        });

        // GET /playlists
        app.MapGet("/playlists", () =>
        {
            List<Playlist> playlists = PlaylistADO.GetAll(dbConn);
            return Results.Ok(playlists);
        });

        // GET /playlists by id
        app.MapGet("/playlists/{id}", (Guid id) =>
        {
            Playlist? playlist = PlaylistADO.GetById(dbConn, id);

            return playlist is not null
                ? Results.Ok(playlist)
                : Results.NotFound(new { message = $"Playlist with Id {id} not found." });
        });

        // PUT /playlists by id
        app.MapPut("/playlists/{id}", (Guid id, PlaylistRequest req) =>
        {
            Playlist? existing = PlaylistADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Playlist updated = new Playlist
            {
                Id = id,
                UserId = req.UserId,
                Name = req.Name,
                Description = req.Description
            };

            PlaylistADO.Update(dbConn, updated);

            return Results.Ok(updated);
        });

        // DELETE /playlists/{id}
        app.MapDelete("/playlists/{id}", (Guid id) => PlaylistADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());
    }
}

public record PlaylistRequest(Guid UserId, string Name, string Description);
