using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;
using SpotifyAPI.Common;

namespace SpotifyAPI.EndPoints;

public static class UserRoleEndpoints
{
    public static void MapUserRoleEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /userRoles 
        app.MapPost("/userRoles", (Guid requesterId, UserRoleRequest req) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            UserRole userRol = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = req.UserId,
                RoleId = req.RoleId
            };

            UserRoleADO.Insert(dbConn, userRol);
            return Results.Created($"/userRoles/{userRol.Id}", userRol);
        });

        // GET /userRoles
        app.MapGet("/userRoles", (Guid requesterId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            List<UserRole> userRol = UserRoleADO.GetAll(dbConn);
            return Results.Ok(userRol);
        });

        // GET /userRoles/roles/{roleId}
        app.MapGet("/userRoles/roles/{roleId}", (Guid requesterId, Guid roleId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            List<UserRole> userRol = UserRoleADO.GetByRole(dbConn, roleId);
            return Results.Ok(userRol);
        });

        // GET /userRoles/users/{userId}
        app.MapGet("/usersRoles/users/{userId}", (Guid requesterId, Guid userId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            List<UserRole> userRol = UserRoleADO.GetByUser(dbConn, userId);
            return Results.Ok(userRol);
        });

        // DELETE /userRoles
        app.MapDelete("/userRoles", (Guid requesterId, Guid userId, Guid roleId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            bool deleted = UserRoleADO.Delete(dbConn, userId, roleId);

            return deleted
                ? Results.NoContent()
                : Results.NotFound("Doesn't exist this relation.");
        });
    }
}
