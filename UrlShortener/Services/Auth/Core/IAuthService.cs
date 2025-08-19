using UrlShortener.Models.DTOs.Auth;

namespace UrlShortener.Services.Auth.Core;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(LoginDto loginDto);
}
