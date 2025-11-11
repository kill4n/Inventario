using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventario.API.Interfaces;
using Inventario.API.Models;
using Inventario.API.Requests;
using Inventario.API.Responses;
using Microsoft.IdentityModel.Tokens;

namespace Inventario.API.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Attempting to log in user: {Username}", request.Username);
        var user = _userRepository.GetUserByUsernameInternal(request.Username);
        if (user == null || user.Password == null)
        {
            _logger.LogWarning("User not found or password is null for username: {Username}", request.Username);
            return null;
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!passwordValid)
        {
            _logger.LogWarning("Invalid password for user: {Username}", request.Username);
            return null;
        }

        var token = GenerateJwtToken(user);
        _logger.LogInformation("User {Username} logged in successfully.", request.Username);
        return new AuthResponse(token, user.Username, user.Email ?? string.Empty);
    }

    public string GenerateJwtToken(User user)
    {
        _logger.LogInformation("Generating JWT token for user: {Username}", user.Username);
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];
        var jwtExpireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        _logger.LogDebug("JWT Claims: {@Claims}", claims);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtExpireMinutes),
            signingCredentials: credentials);

        _logger.LogInformation("JWT token generated for user: {Username}", user.Username);

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}