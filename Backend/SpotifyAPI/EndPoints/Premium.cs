using SpotifyAPI.Repository;
using SpotifyAPI.Model;
using System.Net.Http;
using SpotifyAPI.Services;
using System.Text;
using SpotifyAPI.Infrastructure.Persistence.Entities;

namespace SpotifyAPI.EndPoints;

public static class PremiumEndpoints
{
    public static void MapPremiumEndpoints(this WebApplication app)
    {
        // POST /premium/{userId}
        app.MapPost("/premium/{userId}", async (SpotifyDBConnection dbConn, Guid userId) =>
        {
            UserEntity? userEntity = UserADO.GetById(dbConn, userId);

            if (userEntity == null)
            {
                return Results.NotFound();
            }

            HttpClient client = new HttpClient();

            string json = "{\"name\":\"" + userEntity.Username + "\",\"email\":\"" + userEntity.Email + "\"}";

            await client.PostAsync(
                "http://localhost:8069/spotify/buy_premium",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            return Results.Ok();
        });
    }
}