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
``` JSON
// POST http://localhost:5000/playlists/{id}/add/{id}
```
``` JSON
// DELETE http://localhost:5000/playlistSong/{id}
```
# SongFile


# Roles
```JSON
// GET http://localhost:5000/roles
```
```JSON
// POST http://localhost:5000/roles/
{
  "name" : "rol2",
  "description" : "description2"
}
```

# Permissions
```JSON
// GET http://localhost:5000/permissions
```
```JSON
// POST http://localhost:5000/permissions
{
  "name" : "client",
  "description" : "description1"
}
```

# RolePermissions

```JSON
// GET http://localhost:5000/rolePermissions
```

```JSON
// POST http://localhost:5000/rolePermissions/

{
  "roleId" : "4244ca69-05b4-4e33-b101-a4c9b9b954cb",
  "permissionId" : "bfc8b526-71ed-4d5f-9c8e-2494c0819cff"
}
```
```JSON
Get by Role Id
// GET http://localhost:5000/rolePermissions/role/5b01d447-2b76-40a8-b806-3bc9802ba6bb
```
```JSON
Get by Permission Id
http://localhost:5000/rolePermissions/permission/bfc8b526-71ed-4d5f-9c8e-2494c0819cff
```