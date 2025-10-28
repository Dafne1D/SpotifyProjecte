using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record SongResponse(Guid Id, string Title, string Artist, string Album, int Duration, string Genre, string ImageUrl)
{
    public static SongResponse FromSong(Song song)
    {
        return new SongResponse(song.Id, song.Title, song.Artist, song.Album, song.Duration, song.Genre, song.ImageUrl);
    }
}