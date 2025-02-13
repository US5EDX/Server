using Server.Models.Interfaces;
using Server.Services.Dtos;

namespace Server.Services.Services
{
    public class UsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UsersService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<bool?> UpdatePassword(UpdatePasswordDto updatePassword)
        {
            bool isSuccess = Ulid.TryParse(updatePassword.UserId, out Ulid userId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний id користувача");

            var user = await _userRepository.GetUserById(userId.ToByteArray());

            if (user is null)
                return null;

            if (!HasherService.VerifyPassword(updatePassword.OldPassword, user.Password, user.Salt))
                return false;

            var salt = GeneratorService.GenerateSalt();
            var password = HasherService.GetPBKDF2Hash(updatePassword.NewPassword, salt);

            await _userRepository.UpdatePassword(user, password, salt);

            return true;
        }

        public async Task<bool?> ResetPassword(string id, string requestUserRole)
        {
            bool isSuccess = Ulid.TryParse(id, out Ulid userId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний id користувача");

            var user = await _userRepository.GetUserById(userId.ToByteArray());

            if (user is null)
                return false;

            if (user.Role < int.Parse(requestUserRole))
                return null;

            var salt = GeneratorService.GenerateSalt();

            //For development
            //var password = GeneratorService.GeneratePassword();
            var password = "Test1234";

            var hashedPassword = HasherService.GetPBKDF2Hash(password, salt);

            await _userRepository.UpdatePassword(user, hashedPassword, salt);

            //While development

            //await _emailService.SendEmailAsync(user.Email, "Ваш новий пароль",
            //    "Доброго дня!\n" +
            //    "Не забудьте після входу змінити тимчасовий пароль,\n" +
            //    "і надійно його зберігайте.\n" +
            //    $"Логін: {user.Email}\n" +
            //    $"Тимчасовий пароль: {password}\n" +
            //    "\n\nЗ повагою, адміністратор ДНУ.");

            return true;
        }
    }
}
