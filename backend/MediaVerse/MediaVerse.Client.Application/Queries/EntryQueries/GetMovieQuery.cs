using MediatR;
using MediaVerse.Client.Application.DTOs.EntryDTOs.MovieDTOs;
using MediaVerse.Client.Application.Specifications.EntrySpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.EntryQueries;

public record GetMovieQuery(Guid Id) : IRequest<BaseResponse<GetMovieResponse>>;

public class GetMovieQueryHandler(IRepository<Movie> movieRepository)
    : IRequestHandler<GetMovieQuery, BaseResponse<GetMovieResponse>>
{
    public async Task<BaseResponse<GetMovieResponse>> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetMovieByIdSpecification(request.Id);
        var movie = await movieRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (movie is null) return new BaseResponse<GetMovieResponse>(new NotFoundException());

        var responseBook = new GetMovieResponse(movie.Synopsis, movie.CinematicGenres.Select(bg => bg.Name).ToList());
        return new BaseResponse<GetMovieResponse>(responseBook);
    }
}