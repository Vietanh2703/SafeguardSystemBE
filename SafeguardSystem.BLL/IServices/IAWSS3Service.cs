using Microsoft.AspNetCore.Http;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IAWSS3Service
{
    Task<ResponseDTO> DownloadFileAsync(string fileName);
    Task<ResponseDTO> DeleteFileAsync(string fileName);
    Task<ResponseDTO> UploadFileAsync(string key, Stream fileStream, string contentType);
    Task<ResponseDTO> DefaultUploadFileAsync(IFormFile file, string userId);
    Task<ResponseDTO> GetPreSignedURLAsync(string fileName);
    Task<ResponseDTO> ListUserFilesAsync(string userId);
}