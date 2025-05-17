namespace Server.Services.Converters;

public static class CourseMaskConverter
{
    public static string GetCourseMaskString(byte mask)
    {
        List<string> result = [];

        for (int i = 0; i < 4; i++)
            if ((mask & 1 << i) != 0)
                result.Add((i + 1).ToString());

        return string.Join(", ", result);
    }
}
