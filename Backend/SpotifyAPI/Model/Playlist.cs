namespace Spotify.Model;

public class Playlist
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}

/*CREATE TABLE Playlists (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CONSTRAINT FKPlaylistsUsers FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);*/