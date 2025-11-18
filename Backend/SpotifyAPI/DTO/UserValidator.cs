using SpotifyAPI.Model;
using SpotifyAPI.EndPoints;
using SpotifyAPI.Common;

namespace SpotifyAPI.DTO;

public static class UserValidator
{
    public static Result Validate(UserRequest user)
    {
        if (user.Username == null || user.Username.Count() == 0)
        {
            return Result.Failure("El nom d'usuari és obligatori", "DADA_OBLIGATORIA");
        }
        if (user.Username.Count() > 50)
        {
            return Result.Failure("La longitud del nom d'usuari ha de ser inferior a 50", "LONGITUD_INCORRECTE");
        }
        return Result.Ok();
    }
}