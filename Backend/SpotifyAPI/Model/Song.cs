namespace SpotifyAPI.Model;

public class Song
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Artist { get; set; } = "";
    public string Album { get; set; } = "";
    public int Duration { get; set; }
    public string Genre { get; set; } = "";
    public string ImageUrl { get; set; } = "";
}