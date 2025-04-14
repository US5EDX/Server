using Server.Models.Models;

namespace Server.Services.Services
{
    public class CalcuationService
    {
        public static byte CalculateGroupCourse(Group group)
        {
            int groupCourse = CalculatePassedYearsForGroup(group.AdmissionYear) + 1;

            if (groupCourse < 0 || groupCourse > group.DurationOfStudy)
                groupCourse = 0;

            return (byte)groupCourse;
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
            var currDate = DateTime.Today;
            return (currDate.Month > 7 ? currDate.Year : (currDate.Year - 1)) - admissionYear;
        }
    }
}
