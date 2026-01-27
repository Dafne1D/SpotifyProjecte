using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;
using SpotifyAPI.Common;


namespace SpotifyAPI.EndPoints;

public static class RolePermissionEndpoints
{
    public static void MapRolePermissionEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /rolePermissions
        app.MapPost("/rolePermissions", (Guid requesterId, RolePermissionRequest req) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            RolePermission rolPer = new RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = req.RoleId,
                PermissionId = req.PermissionId
            };

            RolePermissionADO.Insert(dbConn, rolPer);
            return Results.Created($"/rolePermissions/{rolPer.Id}", rolPer);
        });

        // DELETE /rolePermissions
        app.MapDelete("/rolePermissions", (Guid requesterId, Guid roleId, Guid permissionId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);
            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            bool deleted = RolePermissionADO.Delete(dbConn, roleId, permissionId);

            return deleted
                ? Results.NoContent()
                : Results.NotFound("Doesn't exist this relation.");
        });


    }
}
