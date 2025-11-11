using Inventario.API.Interfaces;
using Inventario.API.Models;
using Inventario.API.Requests;
using Inventario.API.Responses;

namespace Inventario.API.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        AuthResponse response = new AuthResponse(
            Token: "dummy-token",
            Username: request.Username,
            Email: "dummy-email@example.com");


        return Task.FromResult(response);
    }

    public string GenerateToken(User user)
    {
        throw new NotImplementedException();
    }
}