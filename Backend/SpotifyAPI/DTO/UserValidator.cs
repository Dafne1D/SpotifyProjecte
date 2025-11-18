using SpotifyAPI.Model;
using SpotifyAPI.EndPoints;
using SpotifyAPI.Common;
using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;


namespace SpotifyAPI.DTO;

public static class UserValidator
{
    public static Result Validate(UserRequest user, SpotifyDBConnection dbConn)
    {
        if (user.Username == null || user.Username.Count() == 0)
        {
            return Result.Failure("El nom d'usuari és obligatori", "DADA_OBLIGATORIA");
        }

        if (user.Username.Count() > 50)
        {
            return Result.Failure("La longitud del nom d'usuari ha de ser inferior a 50", "LONGITUD_INCORRECTE");
        }

        if (UserADO.UsernameExists(dbConn, user.Username))
        {
            return Result.Failure("Aquest nom d'usuari ja existeix", "USERNAME_DUPLICAT");
        }
        
        if (string.IsNullOrEmpty(user.Email))
        {
            return Result.Failure("El correu és obligatori", "EMAIL_OBLIGATORI");
        }

        if (!user.Email.EndsWith("@gmail.com"))
        {
            return Result.Failure("Només es permeten comptes de Gmail", "EMAIL_INVALID");
        }

        if (UserADO.EmailExists(dbConn, user.Email))
        {
            return Result.Failure("Aquest correu ja està registrat", "EMAIL_DUPLICAT");
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            return Result.Failure("La contrasenya és obligatoria", "PASSWORD_OBLIGATORI");
        }

        if (user.Password.Length < 8)
        {
            return Result.Failure("La contrasenya ha de tenir almenys 8 caràcters", "PASSWORD_CURTA");
        }

        bool hasUpper = user.Password.Any(char.IsUpper);
        bool hasLower = user.Password.Any(char.IsLower);
        bool hasDigit = user.Password.Any(char.IsDigit);

        if (!hasUpper || !hasLower || !hasDigit)
        {
            return Result.Failure("La contrasenya ha de contenir majúscules, minúscules i números", "PASSWORD_DÈBIL");
        }


        return Result.Ok();
    }
}