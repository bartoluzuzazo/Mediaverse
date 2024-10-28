using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Client.Application.Specifications.AmaSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.AmaCommands;

public record EndAmaSessionCommand(Guid SessionId) : IRequest<BaseResponse<GetAmaSessionResponse>>;

public class EndAmaSessionCommandHandler(IUserAccessor userAccessor, IRepository<AmaSession> amaSessionRepository, IMapper mapper)
    : IRequestHandler<EndAmaSessionCommand, BaseResponse<GetAmaSessionResponse>>
{
    public async Task<BaseResponse<GetAmaSessionResponse>> Handle(EndAmaSessionCommand request,
        CancellationToken cancellationToken)
    {
        var userId = userAccessor.Id;
        if (userId is null)
        {
            return new BaseResponse<GetAmaSessionResponse>(
                new ProblemException("Authorized user should always have an ID"));
        }

        var amaSessionSpec = new GetSessionWithAuthorSpecification(request.SessionId);
        var amaSession = await amaSessionRepository.FirstOrDefaultAsync(amaSessionSpec, cancellationToken);
        if (amaSession is null)
        {
            return new BaseResponse<GetAmaSessionResponse>(new NotFoundException());
        }

        if (amaSession.Author.UserId != userId)
        {
            return new BaseResponse<GetAmaSessionResponse>(new ForbiddenException());
        }
        amaSession.End= DateTime.Now;
        await amaSessionRepository.SaveChangesAsync(cancellationToken);
        var response = mapper.Map<GetAmaSessionResponse>(amaSession);
        return new BaseResponse<GetAmaSessionResponse>(response);
    }
}