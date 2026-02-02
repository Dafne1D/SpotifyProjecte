using Microsoft.Data.SqlClient;

using SpotifyAPI.Model;
using SpotifyAPI.Services;

namespace SpotifyAPI.Repository;

static class RoleADO
{
    public static void Insert(SpotifyDBConnection dbConn, Role role)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Roles (Id, Name, Description)
                    VALUES (@Id, @Name, @Description)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", role.Id);
        cmd.Parameters.AddWithValue("@Name", role.Name);
        cmd.Parameters.AddWithValue("@Description", role.Description);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

    public static List<Role> GetAll(SpotifyDBConnection dbConn)
    {
        List<Role> roles = new();

        dbConn.Open();
        string sql = "SELECT Id, Name, Description FROM Roles";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            roles.Add(new Role
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2)
            });
        }

        dbConn.Close();
        return roles;
    }

    public static Role? GetById(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Name, Description FROM Roles WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Role? role = null;

        if (reader.Read())
        {
            role = new Role
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2)
            };
        }

        dbConn.Close();
        return role;
    }

    public static void Update(SpotifyDBConnection dbConn, Role role)
    {
        dbConn.Open();

        string sql = @"UPDATE Roles SET
                    Name = @Name,
                    Description = @Description
                    WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", role.Id);
        cmd.Parameters.AddWithValue("@Name", role.Name);
        cmd.Parameters.AddWithValue("@Description", role.Description);

        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine($"{rows} files actualitzades");

        dbConn.Close();
    }

    public static bool Delete(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Roles WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}