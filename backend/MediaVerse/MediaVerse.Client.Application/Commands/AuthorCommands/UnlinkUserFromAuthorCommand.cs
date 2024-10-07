using System.Security.Cryptography;
using MediatR;
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
        var author = await authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author is null)
        {
            return new NotFoundException();
        }

        author.User = null;
        await authorRepository.SaveChangesAsync(cancellationToken);
        return null;
    }
    
}