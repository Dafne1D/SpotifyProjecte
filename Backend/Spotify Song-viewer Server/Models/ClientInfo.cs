namespace Spotify_Song_viewer_Server.Models
{
    internal class ClientInfo
    {
        public string Username { get; }
        public string Song { get; set; }

        public ClientInfo(string username, string song)
        {
            Username = username;
            Song = song;
        }
    }
}
