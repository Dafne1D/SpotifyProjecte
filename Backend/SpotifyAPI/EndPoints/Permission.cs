using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;
using System.Data.Common;

namespace SpotifyAPI.EndPoints;

public static class PermissionEndpoints
{

    public static void MapPermissionEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // GET /permissions
        app.MapGet("/permissions", () =>
        {
            List<Permission> permissions = PermissionADO.GetAll(dbConn);
            return Results.Ok(permissions);
        });
    }
    
}