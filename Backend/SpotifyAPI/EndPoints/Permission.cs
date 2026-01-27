using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;
using SpotifyAPI.Common;

namespace SpotifyAPI.EndPoints;

public static class PermissionEndpoints
{
    public static void MapPermissionEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /permissions
        app.MapPost("/permissions", (Guid requesterId, PermissionRequest req) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            Permission permission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                Description = req.Description
            };

            PermissionADO.Insert(dbConn, permission);

            return Results.Created($"/permissions/{permission.Id}", PermissionResponse.FromPermission(permission));
        });

        // GET /permissions
        app.MapGet("/permissions", (Guid requesterId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ViewUsers))
                return Results.StatusCode(403);

            List<Permission> permissions = PermissionADO.GetAll(dbConn);
            List<PermissionResponse> permissionResponses = new List<PermissionResponse>();
            foreach (Permission permission in permissions)
            {
                permissionResponses.Add(PermissionResponse.FromPermission(permission));
            }
            return Results.Ok(permissionResponses);
        });

        // GET /permissions by id
        app.MapGet("/permissions/{id}", (Guid requesterId, Guid id) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ViewUsers))
                return Results.StatusCode(403);

            Permission? permission = PermissionADO.GetById(dbConn, id);

            return permission is not null
                ? Results.Ok(PermissionResponse.FromPermission(permission))
                : Results.NotFound(new { message = $"Permission with Id {id} not found." });
        });

        // PUT /permissions
        app.MapPut("/permissions/{id}", (Guid requesterId, Guid id, PermissionRequest req) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            Permission? existing = PermissionADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Permission updated = new Permission
            {
                Id = id,
                Name = req.Name,
                Description = req.Description
            };

            PermissionADO.Update(dbConn, updated);

            return Results.Ok(PermissionResponse.FromPermission(updated));
        });

        // DELETE /permissions
        app.MapDelete("/permissions/{id}", (Guid requesterId, Guid id) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            return PermissionADO.Delete(dbConn, id)
                ? Results.NoContent()
                : Results.NotFound();
        });
    }
}