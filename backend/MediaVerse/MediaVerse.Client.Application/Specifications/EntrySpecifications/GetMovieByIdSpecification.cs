using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetMovieByIdSpecification : Specification<Movie>
{
    public GetMovieByIdSpecification(Guid id)
    {
        Query.Where(b => b.Id == id).Include(b => b.CinematicGenres);
    }
}