using Microsoft.Extensions.Configuration;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;
using SpotifyAPI.EndPoints;
using SpotifyAPI.Utils;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddScoped<JswTokenService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtSecretKey"]!)
                ),

            ValidateIssuer = true,
            ValidIssuer = "demo",

            ValidateAudience = true,
            ValidAudience = "public",

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

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

builder.Services.AddScoped(sp =>
    new SpotifyDBConnection(connectionString!)
);

SpotifyDBConnection dbConn = new SpotifyDBConnection(connectionString);

WebApplication SpotifyApp = builder.Build();

SpotifyApp.UseCors("AllowReactApp");
SpotifyApp.UseAuthentication();
SpotifyApp.UseAuthorization();

SpotifyApp.MapMethods("{*path}", new[] { "OPTIONS" }, () => Results.Ok())
   .RequireCors("AllowReactApp");

SpotifyApp.MapPremiumEndpoints();
SpotifyApp.MapUserEndpoints();
SpotifyApp.MapRoleEndpoints();
SpotifyApp.MapUserRoleEndpoints();
SpotifyApp.MapPermissionEndpoints();
SpotifyApp.MapRolePermissionEndpoints();
SpotifyApp.MapSongEndpoints();
SpotifyApp.MapPlaylistEndpoints();


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