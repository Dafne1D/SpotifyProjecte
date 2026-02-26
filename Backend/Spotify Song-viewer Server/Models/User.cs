using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Song_viewer_Server.Models
{
    internal class User
    {
        public Guid Id { get; }
        public string Username { get; } = "";
        public string Email { get; } = "";
        public User(Guid id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
        }
    }
}
