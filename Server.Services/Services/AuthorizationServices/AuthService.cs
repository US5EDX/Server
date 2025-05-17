using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Models.Models;
using Server.Services.Converters;
using Server.Services.Dtos.AuthDtos;
using Server.Services.Dtos.StudentDtos;
using Server.Services.Dtos.WorkerDtos;
using Server.Services.Mappings;
using Server.Services.Services.StaticServices;

namespace Server.Services.Services.AuthorizationServices;

public class AuthService(IUserRepository userRepository, JwtService jwtService)
{
    public async Task<object> LoginOrThrow(LoginDto login, DateTime currDateTime)
    {
        var user = await userRepository.GetByEmail(login.Email);

        if (user is null || !HasherService.VerifyPassword(login.Password, user.Password, user.Salt))
            throw new UnauthorizedException("Неправильна пошта або пароль");

        return await GetAuthorizedUserInfo(user, currDateTime);
    }

    public async Task<object> AutoLoginOrThrow(string refreshToken, DateTime currDateTime)
    {
        ValidateTokenWithThrow(refreshToken);

        var user = await userRepository.GetByToken(refreshToken);

        if (user is null || user.RefreshTokenExpiry < currDateTime)
            throw new UnauthorizedException("Неправильний токен");

        return await GetAuthorizedUserInfo(user, currDateTime);
    }

    public async Task<TokenDto?> RefreshOrThrow(string refreshToken, DateTime currDateTime)
    {
        ValidateTokenWithThrow(refreshToken);

        var user = await userRepository.GetByToken(refreshToken);

        if (user is null || user.RefreshTokenExpiry < currDateTime)
            throw new NotFoundException("Неправильний токен");

        return await GetTokenDto(user, currDateTime);
    }

    public async Task Logout(string refreshToken)
    {
        ValidateTokenWithThrow(refreshToken);

        var user = await userRepository.GetByToken(refreshToken);

        if (user is not null)
            await userRepository.DeleteTokenAsync(user);
    }

    private async Task<TokenDto> GetTokenDto(User user, DateTime currDateTime)
    {
        var newAccessToken = jwtService.GenerateToken(UlidConverter.ByteIdToString(user.UserId), ((byte)user.Role).ToString());
        var refreshToken = user.RefreshToken;

        if (refreshToken is null || user.RefreshTokenExpiry < currDateTime.AddDays(2))
        {
            refreshToken = GeneratorService.GenerateRefreshToken();
            await userRepository.RefreshToken(user, refreshToken, currDateTime.AddDays(14));
        }

        return new TokenDto(newAccessToken, refreshToken);
    }

    private async Task<object> GetAuthorizedUserInfo(User user, DateTime currDateTime)
    {
        var tokens = await GetTokenDto(user, currDateTime);

        var mainUserInfo = new
        {
            UserId = UlidConverter.ByteIdToString(user.UserId),
            user.Email,
            user.Role,
            tokens.AccessToken,
            tokens.RefreshToken,
            StudentInfo = (StudentInfoDto?)null,
            WorkerInfo = (WorkerInfoDto?)null,
        };

        return user.Role switch
        {
            Roles.SupAdmin => mainUserInfo,
            Roles.Student =>
                mainUserInfo with
                {
                    StudentInfo = UserMapper.MapToStudentInfoDto(await userRepository.GetStudent(user.UserId)
                        ?? throw new NotFoundException("Частину даних не вдалось знайти"))
                },
            Roles.Admin or Roles.Lecturer =>
                mainUserInfo with
                {
                    WorkerInfo = UserMapper.MapToWorkerInfoDto(await userRepository.GetWorker(user.UserId)
                        ?? throw new NotFoundException("Частину даних не вдалось знайти"))
                },
            _ => throw new InvalidOperationException("Unknown role")
        };
    }

    private static void ValidateTokenWithThrow(string token)
    {
        if (token.Length != 32) throw new BadRequestException("Неправильний токен");
    }
}
