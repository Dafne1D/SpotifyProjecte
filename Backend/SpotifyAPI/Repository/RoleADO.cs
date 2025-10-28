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
}