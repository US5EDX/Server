using Server.Models.Models;

namespace Server.Services.Services
{
    public class CalcuationService
    {
        public static byte CalculateGroupCourse(Group group)
        {
            int course = CalculatePassedYearsForGroup(group.AdmissionYear) + 1;

            if (course < 0 || course > group.DurationOfStudy)
                course = 0;

            return (byte)course;
        }

        public static int CalculateLastHoldingForGroup(Group group)
        {
            var groupCourse = CalculatePassedYearsForGroup(group.AdmissionYear) + 1;

            if (groupCourse < 0)
                throw new Exception("Неправильно додано групу, не може бути групи за кілька років до вступу");

            if (groupCourse == 0 && group.HasEnterChoise == false)
                groupCourse++;

            if (groupCourse >= group.DurationOfStudy)
                groupCourse = group.DurationOfStudy - 1;

            return group.AdmissionYear + groupCourse;
        }

        private static int CalculatePassedYearsForGroup(short admissionYear)
        {
            var currDate = DateTime.UtcNow;
            return (currDate.Month > 7 ? currDate.Year : (currDate.Year - 1)) - admissionYear;
        }
    }
}
