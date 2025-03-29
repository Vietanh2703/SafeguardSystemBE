namespace SafeguardSystem.Common.DTOs;

public class LoginRequestDTO
{
    public Guid RequestId { get; set; }
    public string Email { get; set; }
    public DateTime DateSent { get; set; }
    public string Status { get; set; }
    public string Reason { get; set; }
}