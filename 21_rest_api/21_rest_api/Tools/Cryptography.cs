using System.Security.Cryptography;
using System.Text;

namespace _21_rest_api.Tools;

public static class Cryptography
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;                // salt
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}
