using Microsoft.AspNetCore.Http;

namespace SafeguardSystem.Common.DTOs;

public class UpdateUserAvatarDTO
{
    public IFormFile Avatar { get; set; }
}