using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface ILocationService
{
    Task<ResponseDTO> GetAllLocationsAsync(int pageNumber, int pageSize);
    Task<ResponseDTO> GetAllLocationAsync();
    Task<ResponseDTO> GetLocationByName(string locationName);
    Task<ResponseDTO> CreateLocationAsync(Guid businessId, LocationDTO locationDTO);

    Task<ResponseDTO> GenerateLocationQrCodeAsync(Guid locationId);
}