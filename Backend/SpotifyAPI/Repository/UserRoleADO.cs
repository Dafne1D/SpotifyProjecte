using SpotifyAPI.Services;
using SpotifyAPI.Model;
using Microsoft.Data.SqlClient;

namespace SpotifyAPI.Repository;

static class UserRoleADO
{
    public static void Insert(SpotifyDBConnection dbConn, UserRole userRole)
    {

        dbConn.Open();

        string sql = @"INSERT INTO UserRoles (Id, UserId, RoleId)
                      VALUES (@Id, @UserId, @RoleId)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", userRole.Id);
        cmd.Parameters.AddWithValue("@UserId", userRole.UserId);
        cmd.Parameters.AddWithValue("@RoleId", userRole.RoleId);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

    public static bool Delete(SpotifyDBConnection dbConn, Guid userId, Guid roleId)
    {
        dbConn.Open();

        string sql = @"DELETE FROM UserRoles
                            WHERE UserId = @UserId
                            AND RoleId = @RoleId";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@RoleId", roleId);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}