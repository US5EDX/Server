﻿using Server.Data.Repositories;
using Server.Models.Models;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<object?> Login(LoginDto login)
        {
            var user = await _userRepository.GetUserByEmail(login.Email);

            if (user == null || !HasherService.VerifyPassword(login.Password, user.Password, user.Salt))
                return null;

            return await GetAuthorizedUserInfo(user);
        }

        public async Task<object?> AutoLogin(string refreshToken)
        {
            var user = await _userRepository.GetUserByToken(refreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            return await GetAuthorizedUserInfo(user);
        }

        public async Task<TokenDto?> Refresh(string refreshToken)
        {
            var user = await _userRepository.GetUserByToken(refreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            return await GetTokenDto(user);
        }

        public async Task Logout(string refreshToken)
        {
            var user = await _userRepository.GetUserByToken(refreshToken);

            if (user != null)
                await _userRepository.DeleteToken(user);
        }

        private async Task<TokenDto> GetTokenDto(User user)
        {
            var newAccessToken = _jwtService.GenerateToken(new Guid(user.UserId).ToString(), user.Role.ToString());
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _userRepository.RefreshToken(user, newRefreshToken, DateTime.UtcNow.AddDays(14));

            return new TokenDto(newAccessToken, newRefreshToken);
        }

        private async Task<object> GetAuthorizedUserInfo(User user)
        {
            var tokens = await GetTokenDto(user);

            if (user.Role == 1)
                return new { tokens.AccessToken, tokens.RefreshToken };

            if (user.Role == 4)
            {
                var student = await _userRepository.GetStudent(user.UserId);

                if (student == null)
                    throw new ArgumentNullException(nameof(student));

                return new
                {
                    Student = UserMapper.MapToStudentInfoDto(student, user.Role),
                    tokens.AccessToken,
                    tokens.RefreshToken
                };

            }

            var worker = await _userRepository.GetWorker(user.UserId);

            if (worker == null)
                throw new ArgumentNullException(nameof(worker));

            return new
            {
                Worker = UserMapper.MapToWorkerInfoDto(worker, user.Role),
                tokens.AccessToken,
                tokens.RefreshToken
            };
        }
    }
}
