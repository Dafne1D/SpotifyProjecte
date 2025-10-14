namespace SpotifyAPI.Model;

public class PlaylistSong
{
    public Guid Id { get; set; }
    public Guid PlaylistId { get; set; }
    public Guid SongId { get; set; }
}