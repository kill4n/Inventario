using Inventario.API.Dto;
using Inventario.API.Models;

namespace Inventario.API.Interfaces;

public interface IUserRepository : IRepository<User, UserDto>
{
    UserDto? GetByUsername(string username);
    User? GetUserByUsernameInternal(string username);
    IEnumerable<User> GetAllUsersInternal();
}