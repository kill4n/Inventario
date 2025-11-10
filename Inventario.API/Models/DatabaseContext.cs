using Inventario.API.Interfaces;
using LiteDB;

namespace Inventario.API.Models;

public class DatabaseContext : IDatabaseContext<LiteDatabase>
{
    private readonly IConfiguration _configuration;
    private readonly LiteDatabase _db;

    public DatabaseContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        _db = new LiteDatabase(connectionString);
    }

    public LiteDatabase GetDatabase()
    {
        return _db;
    }
}