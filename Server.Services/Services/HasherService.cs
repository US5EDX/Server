﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Server.Services.Services
{
    internal class HasherService
    {
        public static string GetPBKDF2Hash(string value, string salt = "3s&YM@B$ecQo8ay$")
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: value,
            salt: Encoding.UTF8.GetBytes(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            return GetPBKDF2Hash(password, storedSalt) == storedHash;
        }
    }
}
