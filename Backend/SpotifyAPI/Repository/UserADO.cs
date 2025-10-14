using Microsoft.Data.SqlClient;
using SpotifyAPI.Services;
using SpotifyAPI.Model;

namespace SpotifyAPI.Repository;

static class UserADO
{
    public static void Insert(SpotifyDBConnection dbConn, User user)
    {
        using var conn = dbConn.Create();
        conn.Open();
        var cmd = new SqlCommand("INSERT INTO Users (Id, Username, Email, Password, Salt, Hash) VALUES (@Id, @Username, @Email, @Password, @Salt, @Hash)", conn);
        cmd.Parameters.AddWithValue("@Id", user.Id);
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@Password", user.Password);
        cmd.Parameters.AddWithValue("@Salt", user.Salt);
        cmd.Parameters.AddWithValue("@Hash", user.Hash);
        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");
        conn.Close();
    }
}
