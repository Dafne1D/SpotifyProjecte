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

        app.MapGet("/playlists/{id}", (Guid id) =>
        {
            Playlist? playlist = PlaylistADO.GetById(dbConn, id);

            return playlist is not null
                ? Results.Ok(playlist)
                : Results.NotFound(new { message = $"Playlist with Id {id} not found." });
        });

    }
}

public record PlaylistRequest(Guid UserId, string Name, string Description);
