namespace SafeguardSystem.Common.DTOs;

public class UserDTO
{
    public string? IdentityNumber { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? Avatar { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
    public string WorkingContract { get; set; }
    public string? Phone { get; set; }
    public DateOnly? BirthDay { get; set; }
}