using Server.Models.CustomExceptions;
using Server.Models.Interfaces;
using Server.Models.Interfaces.ExternalInterfaces;
using Server.Services.Dtos.UserDtos;
using Server.Services.Parsers;
using Server.Services.Services.StaticServices;

namespace Server.Services.Services;

public class UsersService(IUserRepository userRepository, IEmailService emailService)
{
    public async Task UpdatePasswordOrThrow(UpdatePasswordDto updatePassword, string? userId)
    {
        if (userId is null) throw new BadRequestException("Невалідні дані про користувача");

        var byteUserId = UlidIdParser.ParseId(userId);
        var user = await userRepository.GetById(byteUserId) ??
            throw new NotFoundException("Користувача не знайдено");

        if (!HasherService.VerifyPassword(updatePassword.OldPassword, user.Password, user.Salt))
            throw new BadRequestException("Невірний старий пароль");

        var salt = GeneratorService.GenerateSalt();
        var password = HasherService.GetPBKDF2Hash(updatePassword.NewPassword, salt);

        await userRepository.UpdatePassword(user, password, salt);
    }

    public async Task ResetPasswordOrThrow(string userId, string? requestUserRole)
    {
        var byteUserId = UlidIdParser.ParseId(userId);

        var user = await userRepository.GetById(byteUserId) ??
            throw new NotFoundException("Користувача не знайдено");

        if (user.Role <= RoleParser.Parse(requestUserRole))
            throw new ForbidException("Неможливо виконати дію");

        var salt = GeneratorService.GenerateSalt();
        var password = GeneratorService.GeneratePassword();
        var hashedPassword = HasherService.GetPBKDF2Hash(password, salt);

        await userRepository.UpdatePassword(user, hashedPassword, salt);

        await emailService.SendEmailAsync(user.Email, "Ваш новий пароль",
            "Доброго дня!\nНе забудьте після входу змінити тимчасовий пароль,\nі надійно його зберігайте.\n" +
            $"Логін: {user.Email}\nТимчасовий пароль: {password}\n\n\n" +
            "З повагою, адміністратор ДНУ.");
    }
}
