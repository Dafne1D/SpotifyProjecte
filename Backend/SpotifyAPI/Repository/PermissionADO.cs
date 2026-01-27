using System.Security.Permissions;
using Microsoft.Data.SqlClient;

using SpotifyAPI.Model;
using SpotifyAPI.Services;

namespace SpotifyAPI.Repository;

static class PermissionADO
{
    public static void Insert(SpotifyDBConnection dbConn, Permission permission)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Permissions (Id, Name, Description)
                    VALUES (@Id, @Name, @Description)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", permission.Id);
        cmd.Parameters.AddWithValue("@Name", permission.Name);
        cmd.Parameters.AddWithValue("@Description", permission.Description);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }


    public static List<Permission> GetAll(SpotifyDBConnection dbConn)
    {
        List<Permission> permissions = new List<Permission>();

        dbConn.Open();

        string sql = "SELECT Id, Code, Name, Description FROM Permissions";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            permissions.Add(new Permission
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                Description = reader.IsDBNull(3) ? "" : reader.GetString(3)
            });
        }

        dbConn.Close();
        return permissions;
    }


    public static Permission? GetById(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Name, Description FROM Permissions WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Permission? permission = null;

        if (reader.Read())
        {
            permission = new Permission
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2)
            };
        }

        dbConn.Close();
        return permission;
    }

    public static void Update(SpotifyDBConnection dbConn, Permission permission)
    {
        dbConn.Open();

        string sql = @"UPDATE Permissions SET
                    Name = @Name,
                    Description = @Description
                    WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", permission.Id);
        cmd.Parameters.AddWithValue("@Name", permission.Name);
        cmd.Parameters.AddWithValue("@Description", permission.Description);

        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine($"{rows} files actualitzades");

        dbConn.Close();
    }

    public static bool Delete(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Permissions WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }

    public static List<Permission> GetByRole(SpotifyDBConnection dbConn, Guid roleId)
    {  
        List<Permission> list = new();

        dbConn.Open();

        string sql = @"
            SELECT p.Id, p.Code, p.Name, p.Description
            FROM RolePermissions rp
            JOIN Permissions p ON p.Id = rp.PermissionId
            WHERE rp.RoleId = @RoleId
        ";

        using SqlCommand cmd = new(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@RoleId", roleId);

        using SqlDataReader r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new Permission
            {
                Id = r.GetGuid(0),
                Code = r.GetString(1),
                Name = r.GetString(2),
                Description = r.IsDBNull(3) ? "" : r.GetString(3)
            });
        }

        dbConn.Close();
        return list;
    }

}