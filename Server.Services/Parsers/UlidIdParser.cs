using Server.Models.CustomExceptions;

namespace Server.Services.Parsers;

public static class UlidIdParser
{
    public static byte[] ParseId(in string id)
    {
        var isSuccess = Ulid.TryParse(id, out Ulid ulidId);

        if (!isSuccess) throw new CustomInvalidCastException("Невалідний Id");

        return ulidId.ToByteArray();
    }

    public static byte[]? ParseIdWithNull(in string? id)
    {
        if (string.IsNullOrEmpty(id)) return null;

        var isSuccess = Ulid.TryParse(id, out Ulid ulidId);

        if (!isSuccess) throw new CustomInvalidCastException("Невалідний Id");

        return ulidId.ToByteArray();
    }

    public static bool TryParseId(in string id, out byte[]? byteId)
    {
        var isSuccess = Ulid.TryParse(id, out Ulid ulidId);

        if (!isSuccess)
        {
            byteId = default;
            return false;
        }

        byteId = ulidId.ToByteArray();

        return true;
    }
}
