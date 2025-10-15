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
                Salt = req.Salt,
                Hash = req.Hash
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
    }
}

public record UserRequest(string Username, string Email, string Password, string Salt, string Hash);