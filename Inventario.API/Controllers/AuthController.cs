using Inventario.API.Interfaces;
using Inventario.API.Models;
using Inventario.API.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthController(
        ILogger<AuthController> logger,
        IConfiguration configuration,
        IUserRepository userRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null)
        {
            _logger.LogWarning("Login request is null.");
            return BadRequest("Invalid login request.");
        }

        var username = request.Username;
        var password = request.Password;
        //MD5 hash of password
        using var md5 = System.Security.Cryptography.MD5.Create();
        var hashedPasswordBytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        var hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLowerInvariant();

        _logger.LogInformation("Attempting login for user: {Username}", username);
        _logger.LogDebug("Hashed password: {HashedPassword}", hashedPassword);

        _logger.LogInformation("Login");

        var user = _userRepository.GetByUsername(username);
        if (user == null)
        {
            _logger.LogWarning("User not found: {Username}", username);
            return Unauthorized("Invalid username or password.");
        }
        _logger.LogInformation("User found: {Username}", username);
        var users = _userRepository.GetAll();
        _logger.LogInformation("Users retrieved: {Count}", users.Count());


        return Ok(users);
    }
}