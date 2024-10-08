using System.Security.Cryptography;
using MediatR;
using MediaVerse.Client.Application.Specifications.AuthorSpecifications;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Entities;
using MediaVerse.Domain.Exceptions;
using MediaVerse.Domain.Interfaces;

namespace MediaVerse.Client.Application.Commands.AuthorCommands;

public record UnlinkUserFromAuthorCommand(Guid AuthorId) : IRequest<Exception?>;

public class UnlinkUserFromAuthorCommandHandler(IRepository<Author> authorRepository) : IRequestHandler<UnlinkUserFromAuthorCommand, Exception?>
{
    public async Task<Exception?> Handle(UnlinkUserFromAuthorCommand request, CancellationToken cancellationToken)
    {
        var getAuthorWithUserSpecification = new GetAuthorWithUserSpecification(request.AuthorId);
        var author = await authorRepository.FirstOrDefaultAsync(getAuthorWithUserSpecification, cancellationToken);
        if (author is null || author.User is null)
        {
            return new NotFoundException();
        }

        author.User.Authors.Remove(author);
        await authorRepository.SaveChangesAsync(cancellationToken);
        return null;
    }
    
}