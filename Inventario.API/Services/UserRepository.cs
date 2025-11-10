using Inventario.API.Dto;
using Inventario.API.Interfaces;
using Inventario.API.Models;
using LiteDB;

namespace Inventario.API.Services;

public class UserRepository : IRepository<User, UserDto>
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
        return Collection.FindAll().Select(static user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email ?? string.Empty,
        }).ToList();
    }

    public User GetById(string id)
    {
        return Collection.FindById(id);
    }

    public void Add(User item)
    {
        ArgumentNullException.ThrowIfNull(item);
        Collection.Insert(item);
    }

    public void Update(User item)
    {
        ArgumentNullException.ThrowIfNull(item);
        Collection.Update(item);
    }

    public void Delete(string id)
    {
        ArgumentNullException.ThrowIfNull(id);
        Collection.Delete(id);
    }
}