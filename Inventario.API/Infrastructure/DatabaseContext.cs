using Inventario.API.Interfaces;
using LiteDB;

namespace Inventario.API.Models;

public class DatabaseContext : IDatabaseContext<LiteDatabase>, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseContext> _logger;
    private readonly LiteDatabase _db;
    private bool _disposed = false;

    public DatabaseContext(
        IConfiguration configuration, 
        ILogger<DatabaseContext> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        _db = new LiteDatabase(connectionString);
        _logger.LogInformation("DatabaseContext initialized with connection string: {ConnectionString}", connectionString);
    }

    public void Dispose()
    {
        if(!_disposed)
        {
            _db?.Dispose();
            _disposed = true;
            _logger.LogInformation("DatabaseContext disposed.");
        }
    }

    public LiteDatabase GetDatabase()
    {
        return _db;
    }
}