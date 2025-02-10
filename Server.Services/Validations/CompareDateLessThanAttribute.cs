using System.ComponentModel.DataAnnotations;

namespace Server.Services.Validations
{
    public class CompareDateLessThanAttribute : ValidationAttribute
    {
        private readonly string _propToCompare;

        public CompareDateLessThanAttribute(string propToCompare)
        {
            _propToCompare = propToCompare;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var prop = validationContext.ObjectType.GetProperty(_propToCompare);

            if (prop is null)
                throw new ArgumentException("Property with this name not found");

            var propValue = prop.GetValue(validationContext.ObjectInstance);

            if (value is null || propValue is null)
                return ValidationResult.Success;

            if ((DateOnly)value < (DateOnly)propValue)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? "Date must be less than " + _propToCompare);
        }
    }
}
