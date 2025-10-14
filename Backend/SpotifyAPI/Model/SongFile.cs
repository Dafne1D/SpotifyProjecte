namespace SpotifyAPI.Model;

public class SongFile
{
    public Guid Id { get; set; }
    public Guid SongId { get; set; }
    public string Url { get; set; } = "";
}