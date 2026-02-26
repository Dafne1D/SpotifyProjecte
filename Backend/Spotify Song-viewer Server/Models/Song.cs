using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Song_viewer_Server.Models
{
    internal class Song
    {
        public Guid Id { get; }
        public string Title { get; } = "";
        public string Artist { get; } = "";
        public string Album { get; } = "";
        public int Duration { get; }
        public string Genre { get; } = "";
        public string ImageUrl { get; } = "";

        public Song(Guid id, string title, string artist, string album, int duration, string genre, string imageUrl)
        {
            Id = id;
            Title = title;
            Artist = artist;
            Album = album;
            Duration = duration;
            Genre = genre;
            ImageUrl = imageUrl;
        }
    }
}
