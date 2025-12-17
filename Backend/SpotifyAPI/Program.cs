using Microsoft.Extensions.Configuration;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;
using SpotifyAPI.EndPoints;
using SpotifyAPI.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:8081")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

string connectionString = builder.Configuration.GetConnectionString("SpotifyDBConnection") ?? "";
SpotifyDBConnection dbConn = new SpotifyDBConnection(connectionString);

WebApplication SpotifyApp = builder.Build();

SpotifyApp.UseCors("AllowReactApp");

SpotifyApp.MapMethods("{*path}", new[] { "OPTIONS" }, () => Results.Ok())
   .RequireCors("AllowReactApp");

SpotifyApp.MapUserEndpoints(dbConn);
SpotifyApp.MapRoleEndpoints(dbConn);
SpotifyApp.MapSongEndpoints(dbConn);
SpotifyApp.MapPlaylistEndpoints(dbConn);
SpotifyApp.MapPermissionEndpoints(dbConn);
SpotifyApp.MapRolePermissionEndpoints(dbConn);
SpotifyApp.MapUserRoleEndpoints(dbConn);


SpotifyApp.MapGet("/", () =>
{
    try
    {
        bool connState = dbConn.Open();
        return $"Database connection: {connState}";
    }
    catch (SqlException ex)
    {
        return $"Database connection failed: {ex.Message}";
    }
});

SpotifyApp.Run();