using Server.Models.CustomExceptions;

namespace Server.Models.Enums;

public enum DeleteResultEnum { HasDependencies, ValueNotFound, Success }

public static class DeleteResultExtensions
{
    public static void ThrowIfFailed(this DeleteResultEnum result, string notFoundMessage, string dependencyMessage)
    {
        switch (result)
        {
            case DeleteResultEnum.ValueNotFound:
                throw new NotFoundException(notFoundMessage);
            case DeleteResultEnum.HasDependencies:
                throw new BadRequestException(dependencyMessage);
        }
    }
}
