using Ardalis.Specification;
using MediaVerse.Domain.Entities;

namespace MediaVerse.Client.Application.Specifications.AuthorSpecifications;

public class GetAuthorWithPhotoSpecification : Specification<Author>
{
    public GetAuthorWithPhotoSpecification(Guid id)
    {
        Query.Where(a => a.Id.Equals(id))
            .Include(a => a.ProfilePicture)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Ratings)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.CoverPhoto)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.AuthorRole)
            
            //Tymczasowo potrzebne do zdeterminowania typu entry
            
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Book)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Album)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Song)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Game)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Movie)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Series)
            .Include(a => a.WorkOns).ThenInclude(wo => wo.Entry).ThenInclude(e => e.Episode);
    }
}