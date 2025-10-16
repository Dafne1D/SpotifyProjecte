# User
``` JSON
// POST http://localhost:5000/users
{
  "username": "marcsoler897",
  "email": "marcsoler897@gmail.com",
  "password": "patata"
}
```
``` JSON
// GET http://localhost:5000/users
// GET http://localhost:5000/users/id
```
``` JSON
// PUT http://localhost:5000/users/id
{
  "username": "marcsoler8977",
  "email": "marcsoler897@gmail.com",
  "password": "patata2"
}

```
``` JSON
// DELETE http://localhost:5000/users/id
```
# Song
``` JSON
// POST http://localhost:5000/songs

{
  "title" : "Never Gonna Give You Up",
  "artist" : "Rick Astley",
  "album" : "idk",
  "duration" : 120,
  "genre" : "peak",
  "imageurl" : "a"
}
```

``` JSON
// GET http://localhost:5000/songs
// GET http://localhost:5000/songs/id
```

``` JSON
// PUT http://localhost:5000/songs/id

{
  "title" : "Hopes and Dreams",
  "artist" : "Toby Fox",
  "album" : "Undertale OST",
  "duration" : 120,
  "genre" : "ultrapeak",
  "imageurl" : "c"
}
```
``` JSON
// DELETE http://localhost:5000/songs/id
```

# Playlist
``` JSON
// POST http://localhost:5000/playlists
{
  "userId": "",
  "name": "Chill Vibes",
  "description": "Relaxing songs for the weekend"
}
```
``` JSON
// GET http://localhost:5000/playlists
// GET http://localhost:5000/playlists/id
```
``` JSON
// PUT http://localhost:5000/playlists/id
{
  "userId": "82b83cfe-1376-4ed5-af34-7d12a102addd",
  "name": "Workout Mix Updated",
  "description": "Updated playlist description"
}
```
``` JSON
// DELETE http://localhost:5000/playlists/id
```
# PlaylistSong

# SongFile