using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.WorkOnSpecifications;

public class GetWorkOnsByEntryIdSpecification : Specification<WorkOn>
{
    public GetWorkOnsByEntryIdSpecification(Guid id)
    {
        Query.Where(wo => wo.EntryId == id);
    }
}