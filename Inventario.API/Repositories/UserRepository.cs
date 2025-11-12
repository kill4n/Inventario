using Inventario.API.Dto;
using Inventario.API.Interfaces;
using Inventario.API.Models;
using LiteDB;

namespace Inventario.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LiteDatabase _db;
    private readonly ILogger<UserRepository> _logger;
    private ILiteCollection<User> Collection => _db.GetCollection<User>(_collectionName);
    private readonly string _collectionName = "users";

    public UserRepository(
        IDatabaseContext<LiteDatabase> context,
        ILogger<UserRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        _db = context.GetDatabase();
        _logger = logger;
        Collection.EnsureIndex(x => x.Username, true);

        _logger.LogInformation("UserRepository initialized.");
    }

    public IEnumerable<UserDto> GetAll()
    {
        _logger.LogInformation("Retrieving all users.");
        return Collection.FindAll().Select(static user =>
        {
            return new UserDto(
                user.Id?.ToString() ?? string.Empty,
                user.Username,
                user.Email ?? string.Empty
            );
        }).ToList();
    }

    public IEnumerable<User> GetAllUsersInternal()
    {
        _logger.LogInformation("Retrieving all user entities for internal use.");
        return Collection.FindAll().ToList();
    }

    public UserDto? GetById(string id)
    {
        _logger.LogDebug("Getting user by ID: {Id}", id);

        var user = Collection.Find(u => u.Id == new ObjectId(id)).FirstOrDefault();
        if (user == null)
            return null;

        return new UserDto(
            user.Id?.ToString() ?? string.Empty,
            user.Username,
            user.Email ?? string.Empty
            );
    }

    public UserDto? GetByUsername(string username)
    {
        _logger.LogDebug("Getting user by Username: {Username}", username);

        var user = Collection.FindOne(u => u.Username == username);
        if (user == null)
            return null;

        return new UserDto(
            user.Id?.ToString() ?? string.Empty,
            user.Username,
            user.Email ?? string.Empty
            );
    }

    public User? GetUserByUsernameInternal(string username)
    {
        _logger.LogDebug("Getting user entity by Username for authentication: {Username}", username);
        return Collection.FindOne(u => u.Username == username);
    }

    public void Add(User item)
    {
        ArgumentNullException.ThrowIfNull(item);
        Collection.Insert(item);
    }

    public bool Update(User item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return Collection.Update(item);
    }

    public bool Delete(string id)
    {
        ArgumentNullException.ThrowIfNull(id);
        return Collection.Delete(id);
    }
}