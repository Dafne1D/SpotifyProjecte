using Microsoft.Data.SqlClient;
using SpotifyAPI.Services;

namespace SpotifyAPI.Repository;

static class AuthADO
{
    public static List<string> GetUserPermissionCodes(SpotifyDBConnection dbConn, Guid userId)
    {
        List<string> perms = new List<string>();

        dbConn.Open();

        string sql = @"
            SELECT DISTINCT p.Code
            FROM UserRoles ur
            JOIN RolePermissions rp ON rp.RoleId = ur.RoleId
            JOIN Permissions p ON p.Id = rp.PermissionId
            WHERE ur.UserId = @UserId
        ";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@UserId", userId);

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
            perms.Add(reader.GetString(0));

        dbConn.Close();
        return perms;
    }
}
