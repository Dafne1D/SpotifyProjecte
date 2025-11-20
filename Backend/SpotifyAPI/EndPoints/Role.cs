using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;

namespace SpotifyAPI.EndPoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {

        // GET /roles
        app.MapGet("/roles", () =>
        {
            List<Role> roles = RoleADO.GetAll(dbConn);
            List<RoleResponse> roleResponses = new List<RoleResponse>();
            foreach (Role role in roles)
            {
                roleResponses.Add(RoleResponse.FromRole(role));
            }
            return Results.Ok(roleResponses);
        });
    }
}