using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Abstractions;

using SpotifyAPI.Repository;
using SpotifyAPI.Model;
using SpotifyAPI.Services;
using SpotifyAPI.Utils;
using SpotifyAPI.DTO;
using System.Collections.Generic;

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

            return Results.Created($"/songs/{song.Id}", SongResponse.FromSong(song));
        });

        // GET /songs
        app.MapGet("/songs", () =>
        {
            List<Song> songs = SongADO.GetAll(dbConn);
            List<SongResponse> songResponses = new List<SongResponse>();
            foreach (Song song in songs)
            {
                songResponses.Add(SongResponse.FromSong(song));
            }
            return Results.Ok(songResponses);
        });

        // GET /songs Song by id
        app.MapGet("/songs/{id}", (Guid id) =>
        {
            Song? song = SongADO.GetById(dbConn, id);

            return song is not null
                ? Results.Ok(SongResponse.FromSong(song))
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

            return Results.Ok(SongResponse.FromSong(updated));
        });

        // DELETE /songs by id
        app.MapDelete("/songs/{id}", (Guid id) => SongADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

        // POST Upload File by id
        app.MapPost("/songs/{id}/upload", (Guid id, [FromForm] IFormFileCollection files) =>
        {
            IFormFile[] filesArray = files.ToArray();
            if (filesArray == null)
                return Results.BadRequest(new { message = "No files recieved", files });

            Song? song = SongADO.GetById(dbConn, id);
            if (song is null)
                return Results.NotFound(new { message = $"Song with Id {id} not found." });

            FileHandler.InsertFiles(dbConn, id, filesArray);

            return Results.Ok(new { message = "Files successfully uploaded", files });
        })
        .Accepts<IFormFile[]>("multipart/form-data")
        .DisableAntiforgery();

        // DELETE File from Song
        app.MapDelete("/songs/{id}/delete/{fileId}", (Guid id, Guid fileId) =>
        {
            Song? song = SongADO.GetById(dbConn, id);
            if (song is null)
                return Results.NotFound(new { message = $"Song with Id {id} not found." });

            SongFile? songFile = SongFileADO.GetById(dbConn, fileId);
            if (songFile is null)
                return Results.NotFound(new { message = $"Song file with Id {songFile} not found." });

            SongFileADO.Delete(dbConn, fileId);

            return Results.Ok(new { message = "File successfully deleted", songFile });
        });
    }
}