namespace SafeguardSystem.Common.DTOs;

public class UpdateUserDTO
{
    public string? UserName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? WorkingContract { get; set; }
    public string? FullName { get; set; }
    public DateTime Birthday { get; set; }
}