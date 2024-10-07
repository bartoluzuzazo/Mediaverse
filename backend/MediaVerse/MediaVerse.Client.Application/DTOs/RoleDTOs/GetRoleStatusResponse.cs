namespace MediaVerse.Client.Application.DTOs.RoleDTOs;
public class GetRoleStatusResponse
{
    public Guid Id { get; set; }
    public bool IsUsers { get; set; }
    public string Name { get; set; } = null!;
}
