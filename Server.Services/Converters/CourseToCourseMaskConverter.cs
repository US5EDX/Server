namespace Server.Services.Converters;

public static class CourseToCourseMaskConverter
{
    public static byte ConvertToCourseMask(byte course) => (byte)(1 << (course - 1));

    public static byte ConvertToAdjustedCourseMask(byte course, bool hasEnterChoise) =>
        (byte)(1 << (course - ((course == 1 && hasEnterChoise) ? 1 : 0)));
}
