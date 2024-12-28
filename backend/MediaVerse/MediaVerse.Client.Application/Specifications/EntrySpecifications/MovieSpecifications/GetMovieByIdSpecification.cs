using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications.MovieSpecifications;

public class GetMovieByIdSpecification : Specification<Movie>
{
    public GetMovieByIdSpecification(Guid id)
    {
        Query.Where(m => m.Id == id).Include(m => m.CinematicGenres);
    }
}