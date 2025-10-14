using Microsoft.Extensions.Configuration;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

string connectionString = builder.Configuration.GetConnectionString("SpotifyDBConnection") ?? "";
SpotifyDBConnection dbConn = new SpotifyDBConnection(connectionString);

var app = builder.Build();

app.MapGet("/", () =>
{
    try
    {
        using var conn = dbConn.Create();
        conn.Open();
        return $"Database connection: {conn.State}";
    }
    catch (SqlException ex)
    {
        return $"Database connection failed: {ex.Message}";
    }
});

app.Run();