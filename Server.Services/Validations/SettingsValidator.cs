using Server.Models.CustomExceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Server.Services.Validations;

public static class SettingsValidator<T>
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static T GetValidatedOrThrow(JsonElement value)
    {
        try
        {
            var obj = value.Deserialize<T>(_jsonSerializerOptions)!;
            var context = new ValidationContext(obj);
            Validator.ValidateObject(obj, context, validateAllProperties: true);
            return obj;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch
        {
            throw new BadRequestException("Неправильний формат даних");
        }
    }
}
