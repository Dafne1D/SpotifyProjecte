using Microsoft.Extensions.Configuration;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;
using SpotifyAPI.EndPoints;
using SpotifyAPI.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

string connectionString = builder.Configuration.GetConnectionString("SpotifyDBConnection") ?? "";
SpotifyDBConnection dbConn = new SpotifyDBConnection(connectionString);

WebApplication SpotifyApp = builder.Build();

SpotifyApp.MapUserEndpoints(dbConn);
SpotifyApp.MapRoleEndpoints(dbConn);
SpotifyApp.MapSongEndpoints(dbConn);
SpotifyApp.MapPlaylistEndpoints(dbConn);

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