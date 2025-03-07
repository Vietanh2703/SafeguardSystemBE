using Microsoft.AspNetCore.Http;
using SafeguardSystem.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface IAWSS3Service
    {
        Task<ResponseDTO> DownloadFileAsync(string fileName);
        Task<ResponseDTO> DeleteFileAsync(string fileName);
        Task<ResponseDTO> UploadFileAsync(string key, Stream fileStream, string contentType);
        Task<ResponseDTO> DefaultUploadFileAsync(IFormFile file, string userId);
        Task<ResponseDTO> GetPreSignedURLAsync1(string fileName, double duration);
    }
}
