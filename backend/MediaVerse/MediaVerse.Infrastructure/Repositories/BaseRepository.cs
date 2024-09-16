using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediaVerse.Infrastructure.Repositories;

public class BaseRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    private readonly Context _context;

    public BaseRepository(Context dbContext, Context context) : base(dbContext)
    {
        _context = context;
    }

    public BaseRepository(Context dbContext, ISpecificationEvaluator specificationEvaluator, Context context) : base(
        dbContext, specificationEvaluator)
    {
        _context = context;
    }

    public DbSet<T> GetDbSet()
    {
        return _context.Set<T>();
    }
}