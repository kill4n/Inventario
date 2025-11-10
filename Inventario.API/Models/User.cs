namespace Inventario.API.Models;

public class User
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? Email { get; set; }
    public required int Role { get; set; }

}