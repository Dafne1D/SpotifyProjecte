using SpotifyAPI.Infrastructure.Persistence.Entities;
using SpotifyAPI.Infrastructure.Mappers;
using SpotifyAPI.Domain.Entities;
using SpotifyAPI.Services;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;
using SpotifyAPI.Validators;
using SpotifyAPI.Common;

namespace SpotifyAPI.EndPoints;

public static class UserRoleEndpoints
{
    public static void MapUserRoleEndpoints(this WebApplication app)
    {
        // POST /userRoles 
        app.MapPost("/userRoles", (SpotifyDBConnection dbConn, UserRoleRequest req) =>
        {
            Result result = UserRoleValidator.Validate(req);
            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            Guid id = Guid.NewGuid();

            UserRole userRole = new UserRole(req.UserId, req.RoleId);
            UserRoleEntity userRoleEntity = UserRoleMapper.ToEntity(userRole, id);
            UserRoleADO.Insert(dbConn, userRoleEntity);
            return Results.Created($"/userRoles/{userRoleEntity.Id}", userRole);
        });

        // GET /userRoles
        app.MapGet("/userRoles", (SpotifyDBConnection dbConn) =>
        {
            List<UserRoleEntity> userRole = UserRoleADO.GetAll(dbConn);
            return Results.Ok(userRole);
        });

        // GET /userRoles/roles/{roleId}
        app.MapGet("/userRoles/roles/{roleId}", (SpotifyDBConnection dbConn, Guid roleId) =>
        {
            List<UserRoleEntity> userRole = UserRoleADO.GetByRole(dbConn, roleId);
            return Results.Ok(userRole);
        });

        // GET /userRoles/users/{userId}
        app.MapGet("/userRoles/users/{userId}", (SpotifyDBConnection dbConn, Guid userId) =>
        {
            List<UserRoleEntity> userRole = UserRoleADO.GetByUser(dbConn, userId);
            return Results.Ok(userRole);
        });

        // DELETE /userRoles
        app.MapDelete("/userRoles/{userId}/{roleId}", (SpotifyDBConnection dbConn, Guid userId, Guid roleId) =>
        {
            bool deleted = UserRoleADO.Delete(dbConn, userId, roleId);

            return deleted
                ? Results.NoContent()
                : Results.NotFound("Doesn't exist this relation.");
        });
    }
}
