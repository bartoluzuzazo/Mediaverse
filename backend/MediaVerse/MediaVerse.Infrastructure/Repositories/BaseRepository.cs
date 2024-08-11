using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MediaVerse.Infrastructure.Repositories;

public class BaseRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public BaseRepository(Context dbContext) : base(dbContext)
    {
    }

    public BaseRepository(Context dbContext, ISpecificationEvaluator specificationEvaluator) : base(dbContext, specificationEvaluator)
    {
    }
}
