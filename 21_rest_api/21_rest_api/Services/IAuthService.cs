using _21_rest_api.Models.Auth;

namespace _21_rest_api.Services;

public interface IAuthService
{
    Task<AuthResponse> Register(RegisterRequest request);
    Task<AuthResponse> Login(LoginRequest request);
}
