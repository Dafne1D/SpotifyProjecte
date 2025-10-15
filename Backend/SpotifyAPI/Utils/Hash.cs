using System.Security.Cryptography;
using System.Text;

namespace SpotifyAPI.Utils;

public static class Hash
{
    public static string GenerateSalt(int length = 1)
    {
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] saltBytes = new byte[length];
        rng.GetBytes(saltBytes);
        return Convert.ToHexString(saltBytes)[..1];
    }


    public static string ComputeHash(string text, string salt)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(text + salt);
        byte[] hashBytes = sha256.ComputeHash(bytes);
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}