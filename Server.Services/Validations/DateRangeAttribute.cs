using System.ComponentModel.DataAnnotations;

namespace Server.Services.Validations;

public class DateRangeAttribute : RangeAttribute
{
    public DateRangeAttribute(string minDate, string maxDate)
        : base(typeof(DateTime), minDate, maxDate) => ErrorMessage = $"Date must be in range of {minDate} - {maxDate}.";
}
