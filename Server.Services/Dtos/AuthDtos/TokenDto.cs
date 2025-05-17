namespace Server.Services.Dtos.AuthDtos;

public class TokenDto(string token, string refreshToken)
{
    public string AccessToken { get; set; } = token;
    public string RefreshToken { get; set; } = refreshToken;
}
