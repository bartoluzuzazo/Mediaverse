using Microsoft.AspNetCore.Http;

namespace MediaVerse.Client.Application.DTOs.AuthorDTOs;

public class GetAuthorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Bio { get; set; }
    public List<GetAuthorWorkOnsGroupResponse> WorkOns { get; set; }
    public string ProfilePicture { get; set; }

}