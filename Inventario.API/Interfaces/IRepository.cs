namespace Inventario.API.Interfaces;

using System.Collections.Generic;

public interface IRepository<T, V>
{
    IEnumerable<V> GetAll();
    V? GetById(string id);
    void Add(T item);
    bool Update(T item);
    bool Delete(string id);
}