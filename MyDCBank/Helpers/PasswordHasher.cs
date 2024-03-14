using System;
using MyDCBank;
using System.Security.Cryptography;

public static class PasswordHasher
{
    private const int SaltSize = 16; // Change as needed
    private const int HashSize = 20; // Change as needed
    private const int Iterations = 10000; // Change as needed

    public static string HashPassword(string password)
    {
        byte[] salt;
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetBytes(salt = new byte[SaltSize]);
        }

        using (var key = new Rfc2898DeriveBytes(password, salt, Iterations))
        {
            byte[] hash = key.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }
    }

    public static bool VerifyPassword(string password, string base64Hash)
    {
        byte[] hashBytes = Convert.FromBase64String(base64Hash);

        if (hashBytes.Length != SaltSize + HashSize)
        {
            return false; // Invalid hash length
        }

        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        using (var key = new Rfc2898DeriveBytes(password, salt, Iterations))
        {
            byte[] hash = key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false; // Hash mismatch
                }
            }
            return true; // Password verified
        }
    }
}
