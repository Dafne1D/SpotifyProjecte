using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.DTO;
using SpotifyAPI.Common;
using SpotifyAPI.Validators;
using SpotifyAPI.Utils;


namespace SpotifyAPI.EndPoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {

        // POST /users
        app.MapPost("/users", (UserRequest req) =>
        {
            Guid id;
            Result result = UserValidator.Validate(req, dbConn);
            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            Result duplicateCheck = UserADOValidator.Validate(req, dbConn);
            if (!duplicateCheck.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = duplicateCheck.ErrorCode,
                    message = duplicateCheck.ErrorMessage
                });
            }

            id = Guid.NewGuid();
            string salt = Hash.GenerateSalt();
            string hash = Hash.ComputeHash(req.Password, salt);

            User user = new User
            {
                Id = id,
                Username = req.Username,
                Email = req.Email,
                Password = hash,
                Salt = salt
            };

            UserADO.Insert(dbConn, user);
            var listenerRoleId = RoleADO.GetRoleIdByCode(dbConn, "Listener");

            if (listenerRoleId != null)
            {
                UserRoleADO.Insert(dbConn, new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    RoleId = listenerRoleId.Value
                });
            }

            return Results.Created($"/users/{user.Id}", UserResponse.FromUser(user));

        });

        // GET /users
        app.MapGet("/users", (Guid requesterId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ViewUsers))
                return Results.StatusCode(403);
            List<User> users = UserADO.GetAll(dbConn);
            return Results.Ok(users);
        });


        // GET /users User by id
        app.MapGet("/users/{id}", (Guid requesterId, Guid id) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (requesterId != id && !perms.Contains(Permissions.ViewUsers))
                return Results.StatusCode(403);

            User? user = UserADO.GetById(dbConn, id);

            return user is not null
                ? Results.Ok(user)
                : Results.NotFound(new { message = $"User with Id {id} not found." });
        });

        // PUT /users by id
        app.MapPut("/users/{id}", (Guid requesterId, Guid id, UserRequest req) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (requesterId != id && !perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            User? existing = UserADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Result result = UserValidator.Validate(req, dbConn);
            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            Result duplicateCheck = UserADOValidator.Validate(req, dbConn);
            if (!duplicateCheck.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = duplicateCheck.ErrorCode,
                    message = duplicateCheck.ErrorMessage
                });
            }

            string salt = Hash.GenerateSalt();
            string hash = Hash.ComputeHash(req.Password, salt);

            User updated = new User
            {
                Id = id,
                Username = req.Username,
                Email = req.Email,
                Password = hash,
                Salt = salt
            };

            UserADO.Update(dbConn, updated);

            return Results.Ok(updated);
        });


        // DELETE /users/{id}
        app.MapDelete("/users/{id}", (Guid requesterId, Guid id) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            return UserADO.Delete(dbConn, id)
                ? Results.NoContent()
                : Results.NotFound();
        });


        // --------- ROLES ---------

        // POST /users/{userId}/role/{roleId}
        app.MapPost("/users/{userId}/role/{roleId}", (Guid requesterId, Guid userId, Guid roleId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);


            UserRole userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = roleId
            };
            UserRoleADO.Insert(dbConn, userRole);
            return Results.Created($"/users/{userRole.Id}", userRole);
        });

        // DELETE /users/{userId}/role/{roleId}
        app.MapDelete("/users/{userId}/role/{roleId}", (Guid requesterId, Guid userId, Guid roleId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            return UserRoleADO.Delete(dbConn, userId, roleId)
                ? Results.NoContent()
                : Results.NotFound();
        });


        // --------- PLAYLISTS ---------
        app.MapGet("/users/{userId}/playlists", (Guid requesterId, Guid userId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (requesterId != userId && !perms.Contains(Permissions.ViewUsers))
                return Results.StatusCode(403);

            List<Playlist> playlists = UserADO.GetPlaylists(dbConn, userId);
            // S'ha de afegir DTO
            return Results.Ok(playlists);
        });
    }

}