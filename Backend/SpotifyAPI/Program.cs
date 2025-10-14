using Microsoft.Extensions.Configuration;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;
using SpotifyAPI.EndPoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

string connectionString = builder.Configuration.GetConnectionString("SpotifyDBConnection") ?? "";
SpotifyDBConnection dbConn = new SpotifyDBConnection(connectionString);

WebApplication app = builder.Build();

app.MapUserEndpoints(dbConn);

app.MapGet("/", () =>
{
    try
    {
        using SqlConnection conn = dbConn.Create();
        conn.Open();
        return $"Database connection: {conn.State}";
    }
    catch (SqlException ex)
    {
        return $"Database connection failed: {ex.Message}";
    }
});

app.Run();