using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.Repository;
using SpotifyAPI.DTO;

namespace SpotifyAPI.EndPoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /roles
        app.MapPost("/roles", (RoleRequest req) =>
        {
            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Name = req.Name,
                Description = req.Description
            };

            RoleADO.Insert(dbConn, role);

            return Results.Created($"/roles/{role.Id}", RoleResponse.FromRole(role));
        });

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

        // GET /roles by id
        app.MapGet("/roles/{id}", (Guid id) =>
        {
            Role? role = RoleADO.GetById(dbConn, id);

            return role is not null
                ? Results.Ok(RoleResponse.FromRole(role))
                : Results.NotFound(new { message = $"Role with Id {id} not found." });
        });

        // PUT /roles
        app.MapPut("/roles/{id}", (Guid id, RoleRequest req) =>
        {
            Role? existing = RoleADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Role updated = new Role
            {
                Id = id,
                Name = req.Name,
                Description = req.Description
            };

            RoleADO.Update(dbConn, updated);

            return Results.Ok(RoleResponse.FromRole(updated));
        });

        // DELETE /roles
        app.MapDelete("/roles/{id}", (Guid id) => RoleADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());
    }
}