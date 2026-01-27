using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.DTO;
using SpotifyAPI.Common;

namespace SpotifyAPI.EndPoints;

public static class PlaylistEndpoints
{

    public static void MapPlaylistEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /playlists
        app.MapPost("/playlists", (Guid requesterId, PlaylistRequest req) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManagePlaylists))
            return Results.StatusCode(403);

            Playlist playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                UserId = requesterId,
                Name = req.Name,
                Description = req.Description,
                ImageUrl = req.ImageUrl
            };
            PlaylistADO.Insert(dbConn, playlist);
            return Results.Created($"/playlists/{playlist.Id}", playlist);
        });

        // GET /playlists
        app.MapGet("/playlists", (Guid requesterId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ViewPlaylists))
            return Results.StatusCode(403);

            List<Playlist> playlists = PlaylistADO.GetAll(dbConn);
            return Results.Ok(playlists);
        });

        // GET /playlists by id
        app.MapGet("/playlists/{id}", (Guid requesterId, Guid id) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ViewPlaylists))
            return Results.StatusCode(403);

            Playlist? playlist = PlaylistADO.GetById(dbConn, id);

            return playlist is not null
                ? Results.Ok(playlist)
                : Results.NotFound(new { message = $"Playlist with Id {id} not found." });
        });

        // PUT /playlists by id
        app.MapPut("/playlists/{id}", (Guid requesterId, Guid id, PlaylistRequest req) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManagePlaylists))
            return Results.StatusCode(403);

            Playlist? existing = PlaylistADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Playlist updated = new Playlist
            {
                Id = id,
                UserId = requesterId,

                Name = req.Name,
                Description = req.Description,
                ImageUrl = req.ImageUrl
            };

            PlaylistADO.Update(dbConn, updated);

            return Results.Ok(updated);
        });

        // DELETE /playlists/{id}
        app.MapDelete("/playlists/{id}", (Guid requesterId, Guid id) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManagePlaylists))
                return Results.StatusCode(403);

            return PlaylistADO.Delete(dbConn, id)
                ? Results.NoContent()
                : Results.NotFound();
        });


        // POST /playlists/{playlistId}/song/{songId}
        app.MapPost("/playlists/{playlistId}/song/{songId}", (Guid requesterId, Guid playlistId, Guid songId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManagePlaylists))
            return Results.StatusCode(403);

            PlaylistSong playlistsong = new PlaylistSong
            {
                Id = Guid.NewGuid(),
                PlaylistId = playlistId,
                SongId = songId
            };
            PlaylistSongADO.Insert(dbConn, playlistsong);
            return Results.Created($"/playlists/{playlistsong.Id}", playlistsong);
        });

        // GET /playlists/{playlistId}/songs
        app.MapGet("/playlists/{playlistId}/songs", (Guid requesterId, Guid playlistId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ViewPlaylists))
            return Results.StatusCode(403);

            List<Song> songs = PlaylistADO.GetSongs(dbConn, playlistId);
            List<SongResponse> songResponses = new List<SongResponse>();
            foreach (Song song in songs)
            {
                songResponses.Add(SongResponse.FromSong(song));
            }
            return Results.Ok(songResponses);
        });
        // DELETE /playlists/{playlistId}/remove/{songId}
        // app.MapDelete("/playlistSong/{id}", (Guid id) => PlaylistSongADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());
    }
}

public record PlaylistRequest(string Name, string Description, string ImageUrl);

