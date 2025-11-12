using LiteDB;

namespace Inventario.API.Models;

public class User
{
    public ObjectId? Id { get; set; }
    public string UserId => Id?.ToString() ?? string.Empty;
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? Email { get; set; }
    public required string Role { get; set; }

}