using System.ComponentModel.DataAnnotations;

namespace SafeguardSystem.Common.DTOs;

public class OtpDTO
{
    [Required] public string? Otp { get; set; }
}