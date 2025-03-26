using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers;

public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [Route("location-pagings")]
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve all locations with pagination",
        Description = "Fetches a paginated list of all registered locations in the system.")]
    [SwaggerResponse(200, "Successfully retrieved locations.")]
    [SwaggerResponse(400, "Invalid pagination parameters.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetLocations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
    {
        var locations = await _locationService.GetAllLocationsAsync(pageNumber, pageSize);
        return StatusCode(locations.StatusCode, locations);
    }

    [Route("locations")]
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve all locations",
        Description = "Fetches a list of all registered locations in the system.")]
    [SwaggerResponse(200, "Successfully retrieved locations.")]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetAllLocations()
    {
        var locations = await _locationService.GetAllLocationAsync();
        return StatusCode(locations.StatusCode, locations);
    }

    [Route("{locationName}")]
    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve a location by name",
        Description = "Fetches a location by its name.")]
    [SwaggerResponse(200, "Successfully retrieved location.")]
    [SwaggerResponse(404, "Location not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GetLocationByName(string locationName)
    {
        var location = await _locationService.GetLocationByName(locationName);
        return StatusCode(location.StatusCode, location);
    }

    [Route("location")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new location",
        Description = "Creates a new location based on the provided information.")]
    [SwaggerResponse(200, "Location created successfully.")]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> CreateLocation(Guid businessId, [FromBody] LocationDTO locationDTO)
    {
        var result = await _locationService.CreateLocationAsync(businessId, locationDTO);
        return StatusCode(result.StatusCode, result);
    }
    
    [Route("{locationId}")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update a location",
        Description = "Updates an existing location based on the provided information.")]
    [SwaggerResponse(200, "Location updated successfully.")]
    [SwaggerResponse(404, "Location not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> UpdateLocation(Guid locationId, [FromBody] LocationDTO locationDTO)
    {
        var response = await _locationService.UpdateLocationAsync(locationId, locationDTO);
        return StatusCode(response.StatusCode, response);
    }

    
    [Route("{locationId}/delete")]
    [HttpPut]
    [SwaggerOperation(Summary = "Delete a location",
        Description = "Deletes an existing location based on its ID.")]
    [SwaggerResponse(200, "Location deleted successfully.")]
    [SwaggerResponse(404, "Location not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> DeleteLocation(Guid locationId)
    {
        var response = await _locationService.DeleteLocationAsync(locationId);
        return StatusCode(response.StatusCode, response);
    }

    [Route("{locationId}/qr-code")]
    [HttpGet]
    [SwaggerOperation(Summary = "Generate a QR code for a location",
        Description = "Generates a QR code for a location based on its ID.")]
    [SwaggerResponse(200, "QR code generated successfully.")]
    [SwaggerResponse(404, "Location not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    public async Task<IActionResult> GenerateLocationQrCode(Guid locationId)
    {
        var result = await _locationService.GenerateLocationQrCodeAsync(locationId);
        if (!result.IsSuccess) return StatusCode(result.StatusCode, result);

        var qrCodeBytes = (byte[])result.Result;
        return File(qrCodeBytes, "image/png");
    }
}