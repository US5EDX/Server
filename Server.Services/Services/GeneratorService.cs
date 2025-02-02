using System.Security.Cryptography;
using System.Text;

namespace Server.Services.Services
{
    internal class GeneratorService
    {
        private const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
        private const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMBERS = "0123456789";
        private const string SPECIALSYMBOLS = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        public static string GeneratePassword(int minLength = 8, int maxLength = 13)
        {
            string charPool = LOWERCASE + UPPERCASE + NUMBERS + SPECIALSYMBOLS;

            int passwordLength = GetRandomByteNumber(minLength, maxLength);

            StringBuilder password = new StringBuilder();

            int index = GetRandomByteNumber(0, charPool.Length);
            password.Append(charPool[index]);

            for (int i = 1; i < passwordLength; i++)
            {
                index = (index + GetRandomByteNumber(1, charPool.Length)) % charPool.Length;
                password.Append(charPool[index]);
            }

            return password.ToString();
        }

        public static byte[] GenerateSalt()
        {
            var rncCsp = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            rncCsp.GetBytes(salt);

            return salt;
        }

        private static int GetRandomByteNumber(int minValue, int maxValue)
        {
            if (minValue < 0 || maxValue > 256)
                throw new ArgumentOutOfRangeException();

            if (minValue >= maxValue)
                throw new ArgumentException();

            byte[] randomNumber = new byte[1];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return randomNumber[0] % (maxValue - minValue) + minValue;
        }
    }
}
