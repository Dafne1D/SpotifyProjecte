using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using SpotifyAPI.Domain.Entities;
using SpotifyAPI.DTO;
using SpotifyAPI.Common;
using SpotifyAPI.Validators;
using SpotifyAPI.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SpotifyAPI.Infrastructure.Mappers;
using SpotifyAPI.Infrastructure.Persistence.Entities;

namespace SpotifyAPI.EndPoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/login", (SpotifyDBConnection dbConn, LoginRequest request, JswTokenService jwtService) =>
        {
            if (request == null) return Results.BadRequest(new { message = "Credentials must be sended" });
            if (string.IsNullOrEmpty(request.Login)) return Results.BadRequest(new { message = "Login must be sended" });
            if (string.IsNullOrEmpty(request.Password)) return Results.BadRequest(new { message = "Password must be sended" });

            UserJWTResponse? user = JWTADO.GetByLogin(dbConn, request.Login);

            if (user == null)
                return Results.Unauthorized();

            string token = jwtService.GenerateToken(
                userId: user.Id.ToString(),
                email: user.Email,
                issuer: "demo",
                roles: user.Roles,
                audience: "public",
                lifetime: TimeSpan.FromHours(2)
            );

            return Results.Ok(new { token, user });
        });

        // GET /jwt

        app.MapGet("/jwt", (JswTokenService jwtService) =>
        {
            return Results.Ok(jwtService.GenerateToken(
                userId: "user identification",
                email: "anna@exemple.com",
                issuer: "demo",
                roles: ["admin"],
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

        app.MapPost("/users", (SpotifyDBConnection dbConn, UserRequest req, ClaimsPrincipal userVerification) =>
        {
            // if (!userVerification.Identity?.IsAuthenticated ?? true)
            //     return Results.Unauthorized();

            bool isAdmin = userVerification.Claims.Any(c =>
                c.Type == ClaimTypes.Role && c.Value == "admin");

            if (!isAdmin)
                return Results.Forbid();

            Result result = UserValidator.Validate(req);
            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            Guid id = Guid.NewGuid();

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

            User user = new User(req.Username, req.Email, hash, salt);
            UserEntity userEntity = UserMapper.ToEntity(user, id);
            UserADO.Insert(dbConn, userEntity);
            return Results.Created($"/users/{userEntity.Id}", UserResponse.FromUser(user, id));
        });

        // GET /users
        app.MapGet("/users", (SpotifyDBConnection dbConn) =>
        {
            List<UserEntity> users = UserADO.GetAll(dbConn);
            List<UserResponse> userResponse = new List<UserResponse>();
            foreach (UserEntity userEntity in users)
            {
                User user = UserMapper.ToDomain(userEntity);
                userResponse.Add(UserResponse.FromUser(user, userEntity.Id));
            }
            return Results.Ok(userResponse);
        });

        // GET /users User by id
        app.MapGet("/users/{id}", (SpotifyDBConnection dbConn, Guid id) =>
        {
            UserEntity? userEntity = UserADO.GetById(dbConn, id);

            if (userEntity == null)
                return Results.NotFound(new { message = $"User with Id {id} not found." });

            User user = UserMapper.ToDomain(userEntity);

            return Results.Ok(UserResponse.FromUser(user, userEntity.Id));
        });

        // PUT /users by id
        app.MapPut("/users/{id}", (SpotifyDBConnection dbConn, Guid id, UserRequest req) =>
        {
            UserEntity? existing = UserADO.GetById(dbConn, id);

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

            User user = new User(req.Username, req.Email, hash, salt); 
            UserEntity userEntity = UserMapper.ToEntity(user, id);
            UserADO.Update(dbConn, userEntity);
            return Results.Ok(UserResponse.FromUser(user, id));
        });


        // DELETE /users/{id}
        app.MapDelete("/users/{id}", (SpotifyDBConnection dbConn, Guid id) => UserADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

        // --------- ROLES ---------

        // POST /users/{userId}/role/{roleId}
        // app.MapPost("/users/{userId}/role/{roleId}", (SpotifyDBConnection dbConn, Guid userId, Guid roleId) =>
        // {
        //     UserRole userRole = new UserRole
        //     {
        //         Id = Guid.NewGuid(),
        //         UserId = userId,
        //         RoleId = roleId
        //     };
        //     UserRoleADO.Insert(dbConn, userRole);
        //     return Results.Created($"/users/{userRole.Id}", userRole);
        // });

        // // DELETE /users/{userId}/role/{roleId}
        // app.MapDelete("/users/{userId}/role/{roleId}", (SpotifyDBConnection dbConn, Guid userId, Guid roleId) => UserRoleADO.Delete(dbConn, userId, roleId) ? Results.NoContent() : Results.NotFound());

        // // --------- PLAYLISTS ---------
        // app.MapGet("/users/{userId}/playlists", (SpotifyDBConnection dbConn, Guid userId) =>
        // {
        //     List<Playlist> playlists = UserADO.GetPlaylists(dbConn, userId);
        //     // S'ha de afegir DTO
        //     return Results.Ok(playlists);
        // });

    }
    public record LoginRequest(string Login, string Password);
    public record TokenRequest(string Token);
}