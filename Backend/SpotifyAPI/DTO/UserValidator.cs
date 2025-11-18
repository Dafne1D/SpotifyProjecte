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

        return Result.Ok();
    }
}