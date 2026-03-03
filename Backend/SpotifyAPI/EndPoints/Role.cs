using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;

namespace SpotifyAPI.EndPoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {


        // GET /roles
        app.MapGet("/roles", (SpotifyDBConnection dbConn) =>
        {
            List<Role> roles = RoleADO.GetAll(dbConn);
            List<RoleResponse> roleResponses = new List<RoleResponse>();
            foreach (Role role in roles)
            {
                roleResponses.Add(RoleResponse.FromRole(role));
            }
            return Results.Ok(roleResponses);
        });

        // GET /roles/{id}/permissions
        app.MapGet("/roles/{roleId}/permissions", (SpotifyDBConnection dbConn, Guid roleId) =>
        {
            List<RolePermission> rolPer = RolePermissionADO.GetAll(dbConn, roleId);
            return Results.Ok(rolPer);
        });
    }
}