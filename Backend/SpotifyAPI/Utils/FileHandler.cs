using SpotifyAPI.Repository;
using SpotifyAPI.Model;
using SpotifyAPI.Services;

namespace SpotifyAPI.Utils;

public static class FileHandler
{
    public static void InsertFiles(SpotifyDBConnection dbConn, Guid id, IFormFile[] files)
    {
        List<Task> tasks = new List<Task>();
        foreach (IFormFile file in files)
        {
            tasks.Add(Task.Run(() => InsertFile(dbConn, id, file)));
        }
        Task.WaitAll(tasks.ToArray());
    }

    private static async void InsertFile(SpotifyDBConnection dbConn, Guid id, IFormFile file)
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

        ExtractMetadata(filePath);
    }

    private static async Task<string> SaveFile(Guid id, IFormFile file)
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

    public static void ExtractMetadata(string filePath)
    {
        TagLib.File tagFile = TagLib.File.Create(filePath);

        Console.WriteLine($"Extracting Metadata from file {filePath}");
        Console.WriteLine($"Song Title: {tagFile.Tag.Title ?? ""}");
        Console.WriteLine($"Artist: {tagFile.Tag.Performers}");
        Console.WriteLine($"Album: {tagFile.Tag.Album ?? ""}");
        Console.WriteLine($"Duration: {tagFile.Properties.Duration}");
        Console.WriteLine($"Genre: {tagFile.Tag.Genres}");
        Console.WriteLine($"Cover art: {tagFile.Tag.Pictures}");
    }
}