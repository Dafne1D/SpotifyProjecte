using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;

namespace SpotifyAPI.EndPoints;

public static class UserEndpoints
{


    public static void MapUserEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        // POST /users
        app.MapPost("/users", (UserRequest req) =>
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = req.Username,
                Email = req.Email,
                Password = req.Password,
                Salt = req.Salt
            };
            UserADO.Insert(dbConn, user);
            return Results.Created($"/users/{user.Id}", user);
        });

        // GET /users
        app.MapGet("/users", () =>
        {
            List<User> users = UserADO.GetAll(dbConn);
            return Results.Ok(users);
        });

        // GET /users User by id
        app.MapGet("/users/{id}", (Guid id) =>
        {
            User? user = UserADO.GetById(dbConn, id);

            return user is not null
                ? Results.Ok(user)
                : Results.NotFound(new { message = $"User with Id {id} not found." });
        });

        // PUT /users by id
        app.MapPut("/users/{id}", (Guid id, UserRequest req) =>
        {
            User? existing = UserADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            User updated = new User
            {
                Id = id, 
                Username = req.Username,
                Email = req.Email,
                Password = req.Password,
                Salt = req.Salt
            };

            UserADO.Update(dbConn, updated);

            return Results.Ok(updated); 
        });
    }
}

public record UserRequest(string Username, string Email, string Password, string Salt, string Hash);