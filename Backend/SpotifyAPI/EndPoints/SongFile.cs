using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;

namespace SpotifyAPI.EndPoints;

public static class SongFileEndpoints
{
    public static void MapSongFileEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // GET /songs/{songId}/files/{fileId}

        app.MapGet("/songs/{songId}/files/{fileId}", (Guid songId, Guid fileId) =>
        {
            Song? song = SongADO.GetById(dbConn, songId);
            if (song is null)
                return Results.NotFound(new { message = $"Cançó amb ID {songId} no existeix." });

            SongFile? songFile = SongFileADO.GetById(dbConn, fileId);
            if (songFile is null || songFile.SongId != songId)
                return Results.NotFound(new { message = $"Arxiu amb ID {fileId} no existeix." });

            if (!System.IO.File.Exists(songFile.Url))
                return Results.NotFound(new { message = "Arxiu no trobat al disc." });

            byte[] fileBytes = File.ReadAllBytes(songFile.Url);
            string fileName = Path.GetFileName(songFile.Url);
            string contentType = "application/octet-stream";

            return Results.File(fileBytes, contentType, fileName);
        });
    }
}
