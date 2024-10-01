using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace MediaVerse.Domain.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    public DbSet<T> GetDbSet();
}