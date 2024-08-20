using Microsoft.AspNetCore.Http;

namespace MediaVerse.Client.Application.DTOs.AuthorDTOs;

public class GetAuthorResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Bio { get; set; }

    public string Base64Picture { get; set; }

}