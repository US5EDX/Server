using System.Security.Cryptography;
using System.Text;

namespace Server.Services.Services.StaticServices;

public static class GeneratorService
{
    private const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
    private const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NUMBERS = "0123456789";
    private const string SPECIALSYMBOLS = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

    public static string GeneratePassword(int minLength = 8, int maxLength = 13)
    {
#if DEBUG
        return "Test1234";
#endif
        string charPool = LOWERCASE + UPPERCASE + NUMBERS + SPECIALSYMBOLS;

        int passwordLength = GetRandomByteNumber(minLength, maxLength);

        StringBuilder password = new();

        int index = GetRandomByteNumber(0, charPool.Length);
        password.Append(charPool[index]);

        for (int i = 1; i < passwordLength; i++)
        {
            index = (index + GetRandomByteNumber(1, charPool.Length)) % charPool.Length;
            password.Append(charPool[index]);
        }

        return password.ToString();
    }

    public static string GenerateSalt() => Convert.ToBase64String(inArray: GenerateRandomBytes(12));

    public static string GenerateRefreshToken() => Convert.ToBase64String(inArray: GenerateRandomBytes(24));

    public static byte[] GenerateByteUlid() => Ulid.NewUlid().ToByteArray();

    public static string GenerateRegistrationEmailBody(string email, string password) =>
            "Доброго дня!\nНе забудьте після входу змінити тимчасовий пароль,\nі надійно його зберігайте.\n" +
                    $"Ваш логін для входу: {email}\nТимчасовий пароль: {password}\n" +
                    "Вхід через застосунок.\n\n\nЗ повагою, ДНУ.";

    private static int GetRandomByteNumber(int minValue, int maxValue)
    {
        if (minValue < 0 || maxValue > 256) throw new ArgumentOutOfRangeException();

        if (minValue >= maxValue) throw new ArgumentException();

        byte[] randomNumber = GenerateRandomBytes(1);

        return randomNumber[0] % (maxValue - minValue) + minValue;
    }

    private static byte[] GenerateRandomBytes(int bytesCount)
    {
        byte[] randomBytes = new byte[bytesCount];

        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(randomBytes);

        return randomBytes;
    }
}
