using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;

namespace SpotifyAPI.EndPoints;

public static class UserEndpoints
{


    public static void MapUserEndpoints(this WebApplication app, SpotifyDBConnection dbConn)
    {
        app.MapPost("/users", (UserRequest req) =>
        {
            var user = new User
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
    }
}

public record UserRequest(string Username, string Email, string Password, string Salt, string Hash);