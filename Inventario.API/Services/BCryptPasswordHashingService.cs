using Inventario.API.Interfaces;

namespace Inventario.API.Services;

public class BCryptPasswordHashingService : IPasswordHashingService
{
    private readonly ILogger<BCryptPasswordHashingService> _logger;
    
    public BCryptPasswordHashingService(ILogger<BCryptPasswordHashingService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string HashPassword(string password)
    {
        _logger.LogInformation("Hashing password.");
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        _logger.LogInformation("Verifying password.");
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}