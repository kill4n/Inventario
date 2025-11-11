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
    private readonly IPasswordHashingService _passwordHashingService;

    public AuthService(
        IUserRepository userRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IPasswordHashingService passwordHashingService)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
        _passwordHashingService = passwordHashingService;
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

        var passwordValid = _passwordHashingService.VerifyPassword(request.Password, user.Password);
        if (!passwordValid)
        {
            _logger.LogWarning("Invalid password for user: {Username}", request.Username);
            return null;
        }

        var token = await GenerateJwtToken(user);
        _logger.LogInformation("User {Username} logged in successfully.", request.Username);
        return new AuthResponse(
            token,
            user.Id?.ToString() ?? string.Empty,
            user.Username,
            user.Email ?? string.Empty,
            user.Role);
    }

    public async Task<string> GenerateJwtToken(User user)
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
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", user.Id?.ToString() ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role)
        };
        _logger.LogDebug("JWT Claims: {@Claims}", claims);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtExpireMinutes),
            signingCredentials: credentials);

        _logger.LogInformation("JWT token generated for user: {Username}", user.Username);

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));

    }

    public Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation("Registering new user: {Username}", request.Username);
        var hashedPassword = _passwordHashingService.HashPassword(request.Password);

        var newUser = new User
        {
            Username = request.Username,
            Password = hashedPassword,
            Email = request.Email,
            Role = request.Role ?? "user"
        };

        _userRepository.Add(newUser);
        _logger.LogInformation("User {Username} registered successfully.", request.Username);

        var response = new RegisterResponse(
            newUser.Id?.ToString() ?? string.Empty,
            newUser.Username,
            newUser.Email ?? string.Empty,
            newUser.Role);

        return Task.FromResult(response);

    }
}