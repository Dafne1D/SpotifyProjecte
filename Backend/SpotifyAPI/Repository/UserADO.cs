using Microsoft.Data.SqlClient;
using SpotifyAPI.Services;
using SpotifyAPI.Model;

namespace SpotifyAPI.Repository;

static class UserADO
{
    public static void Insert(SpotifyDBConnection dbConn, User user)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Users (Id, Username, Email, Password, Salt)
                      VALUES (@Id, @Username, @Email, @Password, @Salt)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", user.Id);
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@Password", user.Password);
        cmd.Parameters.AddWithValue("@Salt", user.Salt);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }

    public static List<User> GetAll(SpotifyDBConnection dbConn)
    {
        List<User> users = new();

        dbConn.Open();
        string sql = "SELECT Id, Username, Email FROM Users";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new User
            {
                Id = reader.GetGuid(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2)
            });
        }

        dbConn.Close();
        return users;
    }

    public static User? GetById(SpotifyDBConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Username, Email FROM Users WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        User? user = null;

        if (reader.Read())
        {
            user = new User
            {
                Id = reader.GetGuid(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2)
            };
        }

        dbConn.Close();
        return user;
    }

    public static void Update(SpotifyDBConnection dbConn, User user)
    {
        dbConn.Open();

        string sql = @"UPDATE Users
                    SET Username = @Username,
                        Email = @Email,
                        Password = @Password,
                        Salt = @Salt
                    WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", user.Id);
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@Password", user.Password);
        cmd.Parameters.AddWithValue("@Salt", user.Salt);


        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine($"{rows} fila actualitzada.");
        
        dbConn.Close();
    }
}
