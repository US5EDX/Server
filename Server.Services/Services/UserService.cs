using Server.Data.Repositories;
using Server.Services.Dtos;

namespace Server.Services.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool?> UpdatePassword(UpdatePasswordDto updatePassword)
        {
            var user = await _userRepository.GetUserById(updatePassword.UserId);

            if (user == null)
                return null;

            if (!HasherService.VerifyPassword(updatePassword.OldPassword, user.Password, user.Salt))
                return false;

            var salt = GeneratorService.GenerateSalt();
            var password = HasherService.GetPBKDF2Hash(updatePassword.NewPassword, salt);

            await _userRepository.UpdatePassword(user, password, salt);

            return true;
        }
    }
}
