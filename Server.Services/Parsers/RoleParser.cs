using Server.Models.CustomExceptions;
using Server.Models.Enums;

namespace Server.Services.Parsers;

public static class RoleParser
{
    public static Roles Parse(string? stringRoleValue)
    {
        bool isParsed = byte.TryParse(stringRoleValue, out byte requestUserRole);

        if (!isParsed) throw new BadRequestException("Неможливо підтвердити дані");

        return (Roles)requestUserRole;
    }
}
