namespace Inventario.API.Responses;

public record RegisterResponse(
    string UserId,
    string Username,
    string Email,
    string Role);