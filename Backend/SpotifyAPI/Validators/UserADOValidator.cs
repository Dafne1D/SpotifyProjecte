using SpotifyAPI.Model;
using SpotifyAPI.EndPoints;
using SpotifyAPI.Common;
using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;
using SpotifyAPI.DTO;


namespace SpotifyAPI.Validators;

public static class UserADOValidator
{
    public static Result Validate(UserRequest user, SpotifyDBConnection dbConn)
    {
        if (UserADO.UsernameExists(dbConn, user.Username))
            return Result.Failure("Aquest nom d'usuari ja existeix", "USERNAME_DUPLICAT");

        if (UserADO.EmailExists(dbConn, user.Email))
            return Result.Failure("Aquest correu ja està registrat", "EMAIL_DUPLICAT");

        return Result.Ok();
    }
}

