using Ardalis.Specification;

namespace MediaVerse.Domain.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    
}