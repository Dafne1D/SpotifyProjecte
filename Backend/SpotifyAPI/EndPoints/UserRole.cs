using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;

namespace SpotifyAPI.EndPoints;

public static class UserRoleEndpoints
{
    public static void MapUserRoleEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /userRoles 
        app.MapPost("/userRoles", (UserRoleRequest req) =>
        {
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
        app.MapGet("/userRoles", () =>
        {
            List<UserRole> userRol = UserRoleADO.GetAll(dbConn);
            return Results.Ok(userRol);
        });

        // GET /userRoles/roles/{roleId}
        app.MapGet("/userRoles/roles/{roleId}", (Guid roleId) =>
        {
            List<UserRole> userRol = UserRoleADO.GetByRole(dbConn, roleId);
            return Results.Ok(userRol);
        });

        // GET /userRoles/users/{userId}
        app.MapGet("/usersRoles/users/{userId}", (Guid userId) =>
        {
            List<UserRole> userRol = UserRoleADO.GetByUser(dbConn, userId);
            return Results.Ok(userRol);
        });

        // DELETE /userRoles
        app.MapDelete("/userRoles", (Guid userId, Guid roleId) =>
        {
            bool deleted = UserRoleADO.Delete(dbConn, userId, roleId);

            return deleted
                ? Results.NoContent()
                : Results.NotFound("Doesn't exist this relation.");
        });
    }
}
