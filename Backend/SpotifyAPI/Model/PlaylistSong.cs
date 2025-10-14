namespace Spotify.Model;

public class PlaylistSong
{
    public Guid Id { get; set; }
    public Guid PlaylistId { get; set; }
    public Guid SongId { get; set; }
}

/*CREATE TABLE PlaylistSongs (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    PlaylistId UNIQUEIDENTIFIER NOT NULL,
    SongId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FKPlaylistSongsPlaylists FOREIGN KEY (PlaylistId)
        REFERENCES Playlists(Id),
    CONSTRAINT FKPlaylistSongsSongs FOREIGN KEY (SongId)
        REFERENCES Songs(Id)
);*/