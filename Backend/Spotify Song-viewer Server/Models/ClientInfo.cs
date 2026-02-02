namespace Spotify_Song_viewer_Server.Models
{
    internal class ClientInfo
    {
        public User User { get; }
        public Song Song { get; set; }

        public ClientInfo(User user, Song song)
        {
            User = user;
            Song = song;
        }
    }
}
