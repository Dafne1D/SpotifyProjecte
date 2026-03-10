using SpotifyAPI.Model;
using SpotifyAPI.EndPoints;
using SpotifyAPI.Common;
using SpotifyAPI.Repository;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;
using SpotifyAPI.DTO;

namespace SpotifyAPI.Validators;

public static class UserRoleValidator
{
    private const string UserIdRequiredMessage = "El UserId és obligatori";
    private const string UserIdRequiredCode = "DADA_OBLIGATORIA";

    private const string RoleIdRequiredMessage = "El RoleId és obligatori";
    private const string RoleIdRequiredCode = "DADA_OBLIGATORIA";

    public static Result Validate(UserRoleRequest userRole)
    {
        var result = ValidateUserId(userRole);
        if (!result.IsOk) return result;

        result = ValidateRoleId(userRole);
        if (!result.IsOk) return result;

        return Result.Ok();
    }

    private static Result ValidateUserId(UserRoleRequest userRole)
    {
        if (userRole.UserId == Guid.Empty)
        {
            return Result.Failure(UserIdRequiredMessage, UserIdRequiredCode);
        }

        return Result.Ok();
    }

    private static Result ValidateRoleId(UserRoleRequest userRole)
    {
        if (userRole.RoleId == Guid.Empty)
        {
            return Result.Failure(RoleIdRequiredMessage, RoleIdRequiredCode);
        }

        return Result.Ok();
    }
}