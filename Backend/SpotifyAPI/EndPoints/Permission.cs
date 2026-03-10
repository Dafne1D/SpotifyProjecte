using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;

namespace SpotifyAPI.EndPoints;

public static class PermissionEndpoints
{
    public static void MapPermissionEndpoints(this WebApplication app)
    {
        // POST /permissions
        app.MapPost("/permissions", (SpotifyDBConnection dbConn, PermissionRequest req) =>
        {
            Permission permission = new Permission
            {
                Id = Guid.NewGuid(),
                Code = req.Code,
                Name = req.Name,
                Description = req.Description
            };

            PermissionADO.Insert(dbConn, permission);

            return Results.Created($"/permissions/{permission.Id}", PermissionResponse.FromPermission(permission));
        });

        // GET /permissions
        app.MapGet("/permissions", (SpotifyDBConnection dbConn) =>
        {
            List<Permission> permissions = PermissionADO.GetAll(dbConn);
            List<PermissionResponse> permissionResponses = new List<PermissionResponse>();
            foreach (Permission permission in permissions)
            {
                permissionResponses.Add(PermissionResponse.FromPermission(permission));
            }
            return Results.Ok(permissionResponses);
        });

        // GET /permissions by id
        app.MapGet("/permissions/{id}", (SpotifyDBConnection dbConn, Guid id) =>
        {
            Permission? permission = PermissionADO.GetById(dbConn, id);

            return permission is not null
                ? Results.Ok(PermissionResponse.FromPermission(permission))
                : Results.NotFound(new { message = $"Permission with Id {id} not found." });
        });

        // PUT /permissions/{id}
        app.MapPut("/permissions/{id}", (SpotifyDBConnection dbConn, Guid id, PermissionRequest req) =>
        {
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

        // DELETE /permissions/{id}
        app.MapDelete("/permissions/{id}", (SpotifyDBConnection dbConn, Guid id) => PermissionADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());
    }
}