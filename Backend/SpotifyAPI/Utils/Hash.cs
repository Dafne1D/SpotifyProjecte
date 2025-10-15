using System.Security.Cryptography;
using System.Text;

namespace SpotifyAPI.Utils;

public static class Hash
{
    public static string ComputeHash(string text)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hashBytes = sha256.ComputeHash(bytes);
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}
