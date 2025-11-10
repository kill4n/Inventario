namespace Inventario.API.Interfaces;

using System.Collections.Generic;

public interface IRepository<T,V>
{
    IEnumerable<V> GetAll();
    T GetById(string id);
    void Add(T item);
    void Update(T item);
    void Delete(string id);
}