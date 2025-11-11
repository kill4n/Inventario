using System.Threading.Tasks;
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
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService,
        IUserRepository userRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (request == null)
        {
            _logger.LogWarning("Register request is null.");
            return BadRequest("Invalid register request.");
        }
        var existingUser = _userRepository.GetUserByUsernameInternal(request.Username);
        if (existingUser != null)
        {
            _logger.LogWarning("Username {Username} is already taken.", request.Username);
            return Conflict("Username is already taken.");
        }

        var response = _authService.RegisterAsync(request).Result;
        if (response == null)
        {
            _logger.LogError("User registration failed for username: {Username}", request.Username);
            return StatusCode(500, "User registration failed.");
        }

        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null)
        {
            _logger.LogWarning("Login request is null.");
            return BadRequest("Invalid login request.");
        }

        var response = await _authService.LoginAsync(request);

        if (response == null)
        {
            _logger.LogWarning("Invalid username or password for user: {Username}", request.Username);
            return Unauthorized("Invalid username or password.");
        }

        return Ok(response);
    }
}