using SpotifyAPI.Model;
using SpotifyAPI.EndPoints;
using SpotifyAPI.Common;
using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;
using SpotifyAPI.DTO;

namespace SpotifyAPI.Validators;

public static class UserValidator
{
    private const int MinPasswordLength = 8;
    private const int MaxUsernameLength = 50;
    private const string AllowedEmailDomain = "@gmail.com";

    private const string UsernameRequiredMessage = "El nom d'usuari és obligatori";
    private const string UsernameRequiredCode = "DADA_OBLIGATORIA";

    private const string UsernameLengthMessage = "La longitud del nom d'usuari ha de ser inferior a 50";
    private const string UsernameLengthCode = "LONGITUD_INCORRECTE";

    private const string EmailRequiredMessage = "El correu és obligatori";
    private const string EmailRequiredCode = "EMAIL_OBLIGATORI";

    private const string EmailInvalidMessage = "Només es permeten comptes de Gmail";
    private const string EmailInvalidCode = "EMAIL_INVALID";

    private const string PasswordRequiredMessage = "La contrasenya és obligatoria";
    private const string PasswordRequiredCode = "PASSWORD_OBLIGATORI";

    private const string PasswordTooShortMessage = "La contrasenya ha de tenir almenys 8 caràcters";
    private const string PasswordTooShortCode = "PASSWORD_CURTA";

    private const string PasswordWeakMessage = "La contrasenya ha de contenir majúscules, minúscules i números";
    private const string PasswordWeakCode = "PASSWORD_DÈBIL";
    public static Result Validate(UserRequest user, SpotifyDBConnection dbConn)
    {
        var result = ValidateUsername(user);
        if (!result.IsOk) return result;

        result = ValidateEmail(user);
        if (!result.IsOk) return result;

        result = ValidatePassword(user);
        if (!result.IsOk) return result;

        return Result.Ok();
    }
    private static Result ValidateUsername(UserRequest user)
    {
        if (user.Username == null || user.Username.Count() == 0)
        {
            return Result.Failure(UsernameRequiredMessage, UsernameRequiredCode);
        }

        if (user.Username.Count() > MaxUsernameLength)
        {
            return Result.Failure(UsernameLengthMessage, UsernameLengthCode);
        }

        return Result.Ok();
    }

    private static Result ValidateEmail(UserRequest user)
    {
        if (string.IsNullOrEmpty(user.Email))
        {
            return Result.Failure(EmailRequiredMessage, EmailRequiredCode);
        }

        if (!user.Email.EndsWith(AllowedEmailDomain))
        {
            return Result.Failure(EmailInvalidMessage, EmailInvalidCode);
        }

        return Result.Ok();
    }

    private static Result ValidatePassword(UserRequest user)
    {
        if (string.IsNullOrEmpty(user.Password))
        {
            return Result.Failure(PasswordRequiredMessage, PasswordRequiredCode);
        }

        if (user.Password.Length < MinPasswordLength)
        {
            return Result.Failure(PasswordTooShortMessage, PasswordTooShortCode);
        }

        bool hasUpper = user.Password.Any(char.IsUpper);
        bool hasLower = user.Password.Any(char.IsLower);
        bool hasDigit = user.Password.Any(char.IsDigit);

        if (!hasUpper || !hasLower || !hasDigit)
        {
            return Result.Failure(PasswordWeakMessage, PasswordWeakCode);
        }

        return Result.Ok();
    }
}
