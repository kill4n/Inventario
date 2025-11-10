namespace Inventario.API.Interfaces;

using System.Collections.Generic;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T item);
    void Update(T item);
    void Delete(T item);
}