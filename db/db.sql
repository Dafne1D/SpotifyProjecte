CREATE DATABASE SpotifyDB;
Use SpotifyDB;

CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Code NVARCHAR(50) NOT NULL UNIQUE,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255)
);


CREATE TABLE Permissions (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Code NVARCHAR(50) NOT NULL UNIQUE,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255)
);


CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Salt NVARCHAR(1) NOT NULL
);

CREATE TABLE UserRoles (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId)
        REFERENCES Users(Id),
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId)
        REFERENCES Roles(Id),
    CONSTRAINT UQ_UserRoles UNIQUE (UserId, RoleId)
);

CREATE TABLE RolePermissions (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    PermissionId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleId)
        REFERENCES Roles(Id),
    CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionId)
        REFERENCES Permissions(Id),
    CONSTRAINT UQ_RolePermissions UNIQUE (RoleId, PermissionId)
);


CREATE TABLE Songs (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Artist NVARCHAR(100) NOT NULL,
    Album NVARCHAR(100),
    Duration INT,
    Genre NVARCHAR(50),
    ImageUrl NVARCHAR(255)
);

CREATE TABLE Playlists (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    ImageUrl NVARCHAR(255),
    CONSTRAINT FKPlaylistsUsers FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);

CREATE TABLE SongFiles (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    SongId UNIQUEIDENTIFIER NOT NULL,
    Url NVARCHAR(255) NOT NULL,
    CONSTRAINT FKSongsFiles FOREIGN KEY (SongId)
        REFERENCES Songs(Id)
);

CREATE TABLE PlaylistSongs (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    PlaylistId UNIQUEIDENTIFIER NOT NULL,
    SongId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FKPlaylistSongsPlaylists FOREIGN KEY (PlaylistId)
        REFERENCES Playlists(Id),
    CONSTRAINT FKPlaylistSongsSongs FOREIGN KEY (SongId)
        REFERENCES Songs(Id)
);

INSERT INTO Roles (Id, Code, Name, Description) VALUES
(NEWID(), 'Listener', 'Listener', 'Can browse songs and manage own playlists'),
(NEWID(), 'Artist', 'Artist', 'Can manage songs and manage own playlists'),
(NEWID(), 'Admin', 'Admin', 'Full system access including user and role management');

INSERT INTO Permissions (Id, Code, Name, Description) VALUES
(NEWID(), 'VIEW_SONGS', 'View Songs', 'Allows viewing and browsing songs'),
(NEWID(), 'MANAGE_SONGS', 'Manage Songs', 'Allows creating, editing, and deleting songs'),
(NEWID(), 'VIEW_PLAYLISTS', 'View Playlists', 'Allows viewing playlists'),
(NEWID(), 'MANAGE_PLAYLISTS', 'Manage Playlists', 'Allows creating and editing playlists'),
(NEWID(), 'VIEW_USERS', 'View Users', 'Allows viewing user accounts'),
(NEWID(), 'MANAGE_USERS', 'Manage Users', 'Allows managing users and role assignments');

INSERT INTO RolePermissions (Id, RoleId, PermissionId)
SELECT NEWID(), r.Id, p.Id
FROM Roles r
JOIN Permissions p ON
(
    (r.Code = 'Listener' AND p.Code IN (
        'VIEW_SONGS',
        'VIEW_PLAYLISTS',
        'MANAGE_PLAYLISTS'
    )) OR

    (r.Code = 'Artist' AND p.Code IN (
        'VIEW_SONGS',
        'MANAGE_SONGS',
        'VIEW_PLAYLISTS',
        'MANAGE_PLAYLISTS'
    )) OR

    (r.Code = 'Admin')
);

DECLARE @ADMIN_USER UNIQUEIDENTIFIER = '99999999-9999-9999-9999-999999999999';

IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @ADMIN_USER)
INSERT INTO Users (Id, Username, Email, Password, Salt)
VALUES (
  @ADMIN_USER,
  'admin',
  'admin@test.com',
  '935e296d3dab0c9e023396152244cbbc2d222e765995427c9f36f669a4f90284',
  '3'
);

IF NOT EXISTS (
    SELECT 1
    FROM UserRoles ur
    JOIN Roles r ON r.Id = ur.RoleId
    WHERE ur.UserId = '99999999-9999-9999-9999-999999999999'
      AND r.Code = 'Admin'
)
INSERT INTO UserRoles (Id, UserId, RoleId)
SELECT NEWID(), '99999999-9999-9999-9999-999999999999', r.Id
FROM Roles r
WHERE r.Code = 'Admin';
