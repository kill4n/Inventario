namespace Inventario.API.Requests;

public record RegisterRequest(
    string Username,
    string Password,
    string? Email,
    string? Role);