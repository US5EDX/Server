namespace Server.Services.Converters;

public static class UlidConverter
{
    public static string ByteIdToString(byte[] byteId) => new Ulid(byteId).ToString();
}
