using SpotifyAPI.Services;
using SpotifyAPI.Model;
using Microsoft.Data.SqlClient;

namespace SpotifyAPI.Repository;

static class RolePermissionADO
{
    public static void Insert(SpotifyDBConnection dbConn, RolePermission rolePermission)
    {

        dbConn.Open();

        string sql = @"INSERT INTO RolePermissions (Id, RoleId, PermissionId)
                      VALUES (@Id, @RoleId, @PermissionId)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", rolePermission.Id);
        cmd.Parameters.AddWithValue("@RoleId", rolePermission.RoleId);
        cmd.Parameters.AddWithValue("@PermissionId", rolePermission.PermissionId);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

    public static List<RolePermission> GetAll(SpotifyDBConnection dbConn)
    {
        List<RolePermission> rolePermissions = new();

        dbConn.Open();
        string sql = "SELECT Id, RoleId, PermissionId FROM RolePermissions";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            rolePermissions.Add(new Song
            {
                Id = reader.GetGuid(0),
                RoleId = reader.GetGuid(1),
                PermissionId = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return rolePermissions;
    }

    public static bool Delete(SpotifyDBConnection dbConn, Guid roleId, Guid permissionId)
    {
        dbConn.Open();

        string sql = @"DELETE FROM RolePermissions
                            WHERE RoleId = @RoleId
                            AND PermissionId = @PermissionId";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@RoleId", roleId);
        cmd.Parameters.AddWithValue("@PermissionId", permissionId);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}