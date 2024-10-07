using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.Common;
using MediaVerse.Client.Application.DTOs.UserDTOs;
using MediaVerse.Client.Application.Specifications.UserSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.UserQueries;

public record SearchUsersQuery(int Page, int Size, string Query): IRequest<BaseResponse<Page<GetUserResponse>>>;

public class SearchUsersQueryHandler(IMapper mapper,IRepository<User> userRepository) : IRequestHandler<SearchUsersQuery, BaseResponse<Page<GetUserResponse>>>
{
    public async Task<BaseResponse<Page<GetUserResponse>>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var spec = new SearchUserSpecification(request.Query,request.Page, request.Size);
        var users  = await userRepository.ListAsync(spec, cancellationToken);
        var userCount = await userRepository.CountAsync(spec, cancellationToken);
        var response = mapper.Map<List<GetUserResponse>>(users);
        
        var page = new Page<GetUserResponse>(response, request.Page, userCount,request.Size);
        return new BaseResponse<Page<GetUserResponse>>(page);
        
    }
}