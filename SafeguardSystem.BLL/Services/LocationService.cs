using System.Drawing.Imaging;
using QRCoder;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;

    public LocationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDTO> GetAllLocationsAsync(int pageNumber, int pageSize)
    {
        var paginatedLocations = await _unitOfWork.Locations.GetAllLocations(pageNumber, pageSize);
        if (paginatedLocations == null || !paginatedLocations.Any())
            return new ResponseDTO("Empty location in list.", 200);

        var locationDTOs = paginatedLocations.Where(l => !l.IsDeleted)
            .Select(l => new LocationDTO
            {
                Name = l.Name,
                Latitude = l.Latitude,
                Longitude = l.Longitude
            }).ToList();

        return new ResponseDTO("Retrieve location: ", 200, true,
            new PaginatedList<LocationDTO>(locationDTOs, paginatedLocations.Count, pageSize, pageNumber));
    }

    public async Task<ResponseDTO> GetAllLocationAsync()
    {
        var locations = await _unitOfWork.Locations.GetAllLocations();
        if (locations == null || !locations.Any()) return new ResponseDTO("Empty location in list.", 200);
        var locationDTOs = locations.Where(l => !l.IsDeleted)
            .Select(l => new LocationDTO
            {
                Name = l.Name,
                Latitude = l.Latitude,
                Longitude = l.Longitude
            }).ToList();
        return new ResponseDTO("Retrieve location: ", 200, true, locationDTOs);
    }

    public async Task<ResponseDTO> GetLocationByName(string locationName)
    {
        var results = await _unitOfWork.Locations.GetLocationByName(locationName);
        if (results == null) return new ResponseDTO("Location not found", 404);

        var locationDTO = new LocationDTO
        {
            Name = results.Name,
            Latitude = results.Latitude,
            Longitude = results.Longitude
        };

        return new ResponseDTO("Location retrieved successfully", 200, true, locationDTO);
    }

    public async Task<ResponseDTO> CreateLocationAsync(Guid businessId, LocationDTO locationDTO)
    {
        var existingLocation =
            await _unitOfWork.Locations.GetByCoordinatesAsync(locationDTO.Latitude, locationDTO.Longitude);
        if (existingLocation != null) return new ResponseDTO("Retrieve location: ", 200, true, existingLocation);

        var business = await _unitOfWork.Locations.GetLocationByBusinessId(businessId);
        if (business == null) return new ResponseDTO("Business not found", 404);

        var newLocation = new Location
        {
            LocationId = Guid.NewGuid(),
            Latitude = locationDTO.Latitude,
            Longitude = locationDTO.Longitude,
            Name = locationDTO.Name,
            Address = locationDTO.Address,
            Image = locationDTO.image,
            BusinessId = businessId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _unitOfWork.Locations.AddLocation(newLocation);
        await _unitOfWork.SaveChangeAsync();
        return new ResponseDTO("Location created successfully", 201, true, newLocation);
    }
    
    public async Task<ResponseDTO> UpdateLocationAsync(Guid locationId, LocationDTO locationDTO)
    {
        var location = await _unitOfWork.Locations.GetLocationByIdAsync(locationId);
        if (location == null)
        {
            return new ResponseDTO("Location not found", 404);
        }

        location.Name = locationDTO.Name;
        location.Address = locationDTO.Address;
        location.Image = locationDTO.image;
        location.Latitude = locationDTO.Latitude;
        location.Longitude = locationDTO.Longitude;
        location.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.SaveChangeAsync();

        return new ResponseDTO("Location updated successfully", 200, true, location);
    }

    public async Task<ResponseDTO> DeleteLocationAsync(Guid locationId)
    {
        var location = await _unitOfWork.Locations.GetLocationByIdAsync(locationId);
        if (location == null)
        {
            return new ResponseDTO("Location not found", 404);
        }

        var checkpoints = await _unitOfWork.Checkpoints.GetCheckpointsByLocationIdAsync(locationId);
        foreach (var checkpoint in checkpoints)
        {
            await _unitOfWork.Checkpoints.DeleteAsync(checkpoint.CheckpointId);
        }

        location.IsDeleted = true;
        await _unitOfWork.SaveChangeAsync();

        return new ResponseDTO("Location and related checkpoints marked as deleted successfully", 200, true, location);
    }

    public async Task<ResponseDTO> GenerateLocationQrCodeAsync(Guid locationId)
    {
        var location = await _unitOfWork.Locations.GetLocationByIdAsync(locationId);
        if (location == null) return new ResponseDTO("Location not found", 404);

        var coordinates = $"{location.Latitude},{location.Longitude}";
        return new ResponseDTO("QR code generated successfully", 200, true, GenerateQrCode(coordinates));
    }

    private byte[] GenerateQrCode(string data)
    {
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using (var qrCode = new QRCode(qrCodeData))
            {
                using (var qrCodeImage = qrCode.GetGraphic(20))
                {
                    using (var ms = new MemoryStream())
                    {
                        qrCodeImage.Save(ms, ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
            }
        }
    }
}