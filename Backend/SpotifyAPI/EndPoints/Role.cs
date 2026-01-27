using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;
using SpotifyAPI.Common;


namespace SpotifyAPI.EndPoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {


        // GET /roles
        app.MapGet("/roles", (Guid requesterId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);

            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            List<Role> roles = RoleADO.GetAll(dbConn);
            List<RoleResponse> roleResponses = new();
            foreach (Role role in roles)
                roleResponses.Add(RoleResponse.FromRole(role));

            return Results.Ok(roleResponses);
        });

        // GET /roles/{id}/permissions
        app.MapGet("/roles/{roleId}/permissions", (Guid requesterId, Guid roleId) =>
        {
            var perms = AuthADO.GetUserPermissionCodes(dbConn, requesterId);

            if (!perms.Contains(Permissions.ManageUsers))
                return Results.StatusCode(403);

            var permissions = PermissionADO.GetByRole(dbConn, roleId);
            return Results.Ok(permissions);
        });


    }
}