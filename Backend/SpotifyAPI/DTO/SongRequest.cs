using SpotifyAPI.Model;

namespace SpotifyAPI.DTO;

public record SongRequest(string Title, string Artist, string Album, int Duration, string Genre, string ImageUrl)
{
    public Song ToSong(Guid id)
    {
        return new Song
        {
            Id = id,
            Title = Title,
            Artist = Artist,
            Album = Album,
            Duration = Duration,
            Genre = Genre,
            ImageUrl = ImageUrl
        };
    }
}