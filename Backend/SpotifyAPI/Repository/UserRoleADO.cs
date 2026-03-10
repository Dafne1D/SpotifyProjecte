using SpotifyAPI.Infrastructure.Persistence.Entities;
using SpotifyAPI.Infrastructure.Mappers;
using SpotifyAPI.Domain.Entities;
using SpotifyAPI.Services;
using Microsoft.Data.SqlClient;

namespace SpotifyAPI.Repository;

static class UserRoleADO
{
    public static void Insert(SpotifyDBConnection dbConn, UserRoleEntity userRoleEntity)
    {

        dbConn.Open();

        string sql = @"INSERT INTO UserRoles (Id, UserId, RoleId)
                      VALUES (@Id, @UserId, @RoleId)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", userRoleEntity.Id);
        cmd.Parameters.AddWithValue("@UserId", userRoleEntity.UserId);
        cmd.Parameters.AddWithValue("@RoleId", userRoleEntity.RoleId);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

      public static List<UserRoleEntity> GetAll(SpotifyDBConnection dbConn)
    {
        List<UserRoleEntity> userRoles = new List<UserRoleEntity>();

        dbConn.Open();
        string sql = "SELECT Id, UserId, RoleId FROM UserRoles";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            userRoles.Add(new UserRoleEntity
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                RoleId = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return userRoles;
    }
    public static List<UserRoleEntity> GetByRole(SpotifyDBConnection dbConn, Guid roleId)
    {
        List<UserRoleEntity> userRoles = new List<UserRoleEntity>();

        dbConn.Open();

        string sql = @"SELECT Id, UserId, RoleId 
                    FROM UserRoles
                    WHERE RoleId = @RoleId";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@RoleId", roleId);

        using SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            userRoles.Add(new UserRoleEntity
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                RoleId = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return userRoles;
    }

    public static List<UserRoleEntity> GetByUser(SpotifyDBConnection dbConn, Guid userId)
    {
        List<UserRoleEntity> userRoles = new List<UserRoleEntity>();

        dbConn.Open();

        string sql = @"SELECT Id, UserId, RoleId 
                    FROM UserRoles
                    WHERE UserId = @UserId";
        
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@UserId", userId);

        using SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            userRoles.Add(new UserRoleEntity
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