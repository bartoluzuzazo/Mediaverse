using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.Specifications.GenresSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.GenresQueries;

public record SearchMusicGenresQuery(int Page, int Size, string Query): IRequest<BaseResponse<Page<string>>>;

public class SearchMusicGenresQueryHandler(IMapper mapper, IRepository<MusicGenre> musicGenreRepository) : IRequestHandler<SearchMusicGenresQuery, BaseResponse<Page<string>>>
{
    public async Task<BaseResponse<Page<string>>> Handle(SearchMusicGenresQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchMusicGenresSpecification(request.Query,request.Page, request.Size);
        var genres  = await musicGenreRepository.ListAsync(spec, cancellationToken);
        var genresCount = await musicGenreRepository.CountAsync(spec, cancellationToken);
        var response = mapper.Map<List<string>>(genres.Select(g => g.Name));
        
        var page = new Page<string>(response, request.Page, genresCount,request.Size);
        return new BaseResponse<Page<string>>(page);
        
    }
}