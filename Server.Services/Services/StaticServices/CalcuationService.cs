using Server.Models.Models;

namespace Server.Services.Services.StaticServices;

public static class CalcuationService
{
    public static byte CalculateGroupCourse(Group group)
    {
        int groupCourse = CalculateStudyYear(group.AdmissionYear, 7, DateTime.Today);

        if (groupCourse < 0 || groupCourse > group.DurationOfStudy)
            groupCourse = 0;

        return (byte)groupCourse;
    }

    public static int CalculateLastHoldingForGroup(Group group)
    {
        var groupCourse = CalculateStudyYear(group.AdmissionYear, 7, DateTime.Today);

        if (groupCourse < 0)
            throw new InvalidOperationException("Неправильно додано групу, не може бути групи за кілька років до вступу");

        if (groupCourse == 0 && group.HasEnterChoise == false)
            groupCourse++;

        if (groupCourse >= group.DurationOfStudy)
            groupCourse = group.DurationOfStudy - 1;

        return group.AdmissionYear + groupCourse;
    }

    public static int CalculateStudyYear(short admissionYear, int borderMonth, DateTime currDate) =>
        (currDate.Month > borderMonth ? currDate.Year : currDate.Year - 1) - admissionYear + 1;
}
