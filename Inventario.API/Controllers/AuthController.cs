using Inventario.API.Interfaces;
using Inventario.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IRepository<User> _userRepository;

    public AuthController(
        ILogger<AuthController> logger,
        IConfiguration configuration,
        IRepository<User> userRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    }

    [HttpPost]
    [Route("login")]
    public IActionResult? Login()
    {
        _logger.LogInformation("Login");
        var users = _userRepository.GetAll();

        return Ok(users);
    }
}