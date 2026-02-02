using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;

namespace SpotifyAPI.EndPoints;

public static class RolePermissionEndpoints
{
    public static void MapRolePermissionEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /rolePermissions  (Asignar permiso a rol)
        app.MapPost("/rolePermissions", (RolePermissionRequest req) =>
        {
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
        app.MapDelete("/rolePermissions", (Guid roleId, Guid permissionId) =>
        {
            bool deleted = RolePermissionADO.Delete(dbConn, roleId, permissionId);

            return deleted
                ? Results.NoContent()
                : Results.NotFound("Doesn't exist this relation.");
        });
    }
}
