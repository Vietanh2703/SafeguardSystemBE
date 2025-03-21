using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices
{
    public interface ILocationService
    {
        Task<ResponseDTO> GetAllLocationsAsync(int pageNumber, int pageSize);
        Task<ResponseDTO> GetAllLocationAsync();
        Task<ResponseDTO> GetLocationByName(string locationName);
        Task<ResponseDTO> CreateLocationAsync(Guid businessId,LocationDTO locationDTO);

        Task<ResponseDTO> GenerateLocationQrCodeAsync(Guid locationId);
    }
}
