namespace Inventario.API.Interfaces;

public interface IDatabaseContext<T>
{
    T GetDatabase();
}