namespace Inventario.API.Responses;

public record AuthResponse(
    string Token,
    string UserId);