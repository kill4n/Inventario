using Inventario.API.Models;
using Inventario.API.Requests;
using Inventario.API.Responses;

namespace Inventario.API.Interfaces;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<string> GenerateJwtToken(User user);
}