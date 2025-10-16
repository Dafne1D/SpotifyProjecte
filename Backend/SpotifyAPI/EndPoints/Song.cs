using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;
using Microsoft.AspNetCore.Mvc;

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

        // POST Upload File by id
        app.MapPost("/songs/{id}/upload", async (Guid id, [FromForm] List<IFormFile> files) =>
        {
            if (files == null || files.Count == 0)
                return Results.BadRequest(new { message = "No files recieved", files });

            Song? song = SongADO.GetById(dbConn, id);
            if (song is null)
                return Results.NotFound(new { message = $"Song with Id {id} not found." });

            InsertFiles(dbConn, id, files);

            return Results.Ok(new { message = "Files successfully uploaded", files });
        })
        .Accepts<IFormFile[]>("multipart/form-data")
        .DisableAntiforgery();
    }

    public static async void InsertFiles(SpotifyDBConnection dbConn, Guid id, List<IFormFile> files)
    {
        foreach (IFormFile file in files)
        {
            Console.WriteLine($"PROCESSING FILE {file.Name}");
            string filePath = await SaveFile(id, file);

            SongFile songFile = new SongFile
            {
                Id = Guid.NewGuid(),
                SongId = id,
                Url = filePath
            };

            SongFileADO.Insert(dbConn, songFile);

            Console.WriteLine($"FILE {file.Name} FINISHED PROCESSING");
        }
    }

    public static async Task<string> SaveFile(Guid id, IFormFile file)
    {
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string fileName = $"{id}_{Path.GetFileName(file.FileName)}";
        string filePath = Path.Combine(uploadsFolder, fileName);

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }
}

public record SongRequest(string Title, string Artist, string Album, int Duration, string Genre, string ImageUrl);