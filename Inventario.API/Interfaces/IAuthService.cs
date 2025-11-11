using Inventario.API.Models;
using Inventario.API.Requests;
using Inventario.API.Responses;

namespace Inventario.API.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    string GenerateToken(User user);
}