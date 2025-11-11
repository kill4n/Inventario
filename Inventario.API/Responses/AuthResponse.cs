namespace Inventario.API.Responses;

public record AuthResponse(
    string Token,
    string Username,
    string Email);