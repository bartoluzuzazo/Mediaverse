using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.EntrySpecifications;

public class GetEntryTypeSpecification : SingleResultSpecification<Entry>
{
    public GetEntryTypeSpecification(Guid id)
    {
        Query.Where(e => e.Id == id);
    }
}