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

      public static List<UserRole> GetAll(SpotifyDBConnection dbConn)
    {
        List<UserRole> userRoles = new List<UserRole>();

        dbConn.Open();
        string sql = "SELECT Id, UserId, RoleId FROM UserRoles";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            userRoles.Add(new UserRole
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                RoleId = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return userRoles;
    }
    public static List<UserRole> GetByRole(SpotifyDBConnection dbConn, Guid roleId)
    {
        List<UserRole> userRoles = new List<UserRole>();

        dbConn.Open();

        string sql = @"SELECT Id, UserId, RoleId 
                    FROM UserRoles
                    WHERE RoleId = @RoleId";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@RoleId", roleId);

        using SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            userRoles.Add(new UserRole
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                RoleId = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return userRoles;
    }

    public static List<UserRole> GetByUser(SpotifyDBConnection dbConn, Guid userId)
    {
        List<UserRole> userRoles = new List<UserRole>();

        dbConn.Open();

        string sql = @"SELECT Id, UserId, RoleId 
                    FROM UserRoles
                    WHERE UserId = @UserId";
        
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@UserId", userId);

        using SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            userRoles.Add(new UserRole
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                RoleId = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return userRoles;
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