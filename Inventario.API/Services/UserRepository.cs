using Inventario.API.Interfaces;
using Inventario.API.Models;
using LiteDB;

namespace Inventario.API.Services;

public class UserRepository:IRepository<User>
{
    private readonly IDatabaseContext<LiteDatabase> _context;
    private readonly LiteDatabase _db;
    
    public UserRepository(IDatabaseContext<LiteDatabase> context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _db = _context.GetDatabase();
    }

    public IEnumerable<User> GetAll()
    {
        var collection = _db.GetCollection<User>("users");
        return collection.FindAll().ToList();
    }

    public User GetById(int id)
    {
        var collection = _db.GetCollection<User>("users");
        return collection.FindById(id);
    }

    public void Add(User item)
    {
        var collection = _db.GetCollection<User>("users");
        collection.Insert(item);
    }

    public void Update(User item)
    {
        var collection = _db.GetCollection<User>("users");
        collection.Update(item);
    }

    public void Delete(User item)
    {
        var collection = _db.GetCollection<User>("users");
        collection.Delete(item.Id);
    }
}