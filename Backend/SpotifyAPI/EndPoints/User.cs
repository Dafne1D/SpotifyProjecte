using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Model;
using SpotifyAPI.DTO;
using SpotifyAPI.Common;
using SpotifyAPI.Validators;
using SpotifyAPI.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace SpotifyAPI.EndPoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {

        // GET /jwt

        app.MapGet("/jwt", (JswTokenService jwtService) =>
        {
            return Results.Ok(jwtService.GenerateToken(
                userId: "user identification",
                email: "anna@exemple.com",
                issuer: "demo",
                role: "admin",
                audience: "public",
                lifetime: TimeSpan.FromHours(2)));
        }).WithTags("Users");

        // https://www.jwt.io/

        app.MapPost("/debug/token", (
            TokenRequest request,
            JswTokenService jwtService) =>
        {
            try
            {
                List<Claim> claims = jwtService.ValidateAndGetClaimsFromToken(request.Token);

                List<object> decodedClaims = new List<object>();

                foreach (Claim claim in claims)
                {
                    decodedClaims.Add(new
                    {
                        type = claim.Type,
                        value = claim.Value
                    });
                }

                return Results.Ok(new
                {
                    valid = true,
                    claims = decodedClaims
                });
            }
            catch (SecurityTokenExpiredException)
            {
                return Results.Unauthorized();
            }
            catch (SecurityTokenException)
            {
                return Results.BadRequest("Token invalid or manipulated");
            }
        }).WithTags("Debug");

        app.MapGet("/admin-data-manual", (ClaimsPrincipal user) =>
        {
            if (!user.Identity?.IsAuthenticated ?? true)
                return Results.Unauthorized();

            bool isAdmin = user.Claims.Any(c =>
                c.Type == ClaimTypes.Role && c.Value == "admin");

            if (!isAdmin)
                return Results.Forbid();

            return Results.Ok("Només admins (manual)");
        });

        // POST /users
        app.MapPost("/users", (SpotifyDBConnection dbConn, UserRequest req) =>
        {
            Guid id;
            Result result = UserValidator.Validate(req);
            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            Result duplicateCheck = UserADOValidator.Validate(req, dbConn);
            if (!duplicateCheck.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = duplicateCheck.ErrorCode,
                    message = duplicateCheck.ErrorMessage
                });
            }

            id = Guid.NewGuid();
            string salt = Hash.GenerateSalt();
            string hash = Hash.ComputeHash(req.Password, salt);

            User user = new User
            {
                Id = id,
                Username = req.Username,
                Email = req.Email,
                Password = hash,
                Salt = salt
            };

            UserADO.Insert(dbConn, user);
            return Results.Created($"/users/{user.Id}", UserResponse.FromUser(user));

        });

        // GET /users
        app.MapGet("/users", (SpotifyDBConnection dbConn) =>
        {
            List<User> users = UserADO.GetAll(dbConn);
            return Results.Ok(users);
        });

        // GET /users User by id
        app.MapGet("/users/{id}", (SpotifyDBConnection dbConn, Guid id) =>
        {
            User? user = UserADO.GetById(dbConn, id);

            return user is not null
                ? Results.Ok(user)
                : Results.NotFound(new { message = $"User with Id {id} not found." });
        });

        // PUT /users by id
        app.MapPut("/users/{id}", (SpotifyDBConnection dbConn, Guid id, UserRequest req) =>
        {
            User? existing = UserADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Result result = UserValidator.Validate(req);
            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            Result duplicateCheck = UserADOValidator.Validate(req, dbConn);
            if (!duplicateCheck.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = duplicateCheck.ErrorCode,
                    message = duplicateCheck.ErrorMessage
                });
            }

            string salt = Hash.GenerateSalt();
            string hash = Hash.ComputeHash(req.Password, salt);

            User updated = new User
            {
                Id = id,
                Username = req.Username,
                Email = req.Email,
                Password = hash,
                Salt = salt
            };

            UserADO.Update(dbConn, updated);

            return Results.Ok(updated);
        });


        // DELETE /users/{id}
        app.MapDelete("/users/{id}", (SpotifyDBConnection dbConn, Guid id) => UserADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

        // --------- ROLES ---------

        // POST /users/{userId}/role/{roleId}
        app.MapPost("/users/{userId}/role/{roleId}", (SpotifyDBConnection dbConn, Guid userId, Guid roleId) =>
        {
            UserRole userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = roleId
            };
            UserRoleADO.Insert(dbConn, userRole);
            return Results.Created($"/users/{userRole.Id}", userRole);
        });

        // DELETE /users/{userId}/role/{roleId}
        app.MapDelete("/users/{userId}/role/{roleId}", (SpotifyDBConnection dbConn, Guid userId, Guid roleId) => UserRoleADO.Delete(dbConn, userId, roleId) ? Results.NoContent() : Results.NotFound());

        // --------- PLAYLISTS ---------
        app.MapGet("/users/{userId}/playlists", (SpotifyDBConnection dbConn, Guid userId) =>
        {
            List<Playlist> playlists = UserADO.GetPlaylists(dbConn, userId);
            // S'ha de afegir DTO
            return Results.Ok(playlists);
        });

    }
    public record TokenRequest(string Token);
}