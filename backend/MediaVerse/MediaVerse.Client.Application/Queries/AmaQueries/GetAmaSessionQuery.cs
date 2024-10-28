﻿using AutoMapper;
using MediatR;
using MediaVerse.Client.Application.DTOs.AmaDTOs;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Queries.AmaQueries;

public record GetAmaSessionQuery(Guid SessionId) : IRequest<BaseResponse<GetAmaSessionResponse>>;

public class GetAmaSessionQueryHandler(IRepository<AmaSession> amaSessionRepository, IMapper mapper) : IRequestHandler<GetAmaSessionQuery, BaseResponse<GetAmaSessionResponse>>
{
    public async Task<BaseResponse<GetAmaSessionResponse>> Handle(GetAmaSessionQuery query, CancellationToken cancellationToken)
    {
        var amaSession = await amaSessionRepository.GetByIdAsync(query.SessionId, cancellationToken);
        if (amaSession is null)
        {
            return new BaseResponse<GetAmaSessionResponse>(new NotFoundException());
        }
        var response = mapper.Map<GetAmaSessionResponse>(amaSession);
        return new BaseResponse<GetAmaSessionResponse>(response);
    }
}