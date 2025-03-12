using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Web.Controllers
{
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [Route("locations")]
        [HttpGet]
        public async Task<IActionResult> GetLocations([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var locations = await _locationService.GetAllLocationsAsync(pageNumber, pageSize);
            return StatusCode(locations.StatusCode, locations);
        }

        [Route("search-location/{locationName}")]
        [HttpGet]
        public async Task<IActionResult> GetLocationByName(string locationName)
        {
            var location = await _locationService.GetLocationByName(locationName);
            return StatusCode(location.StatusCode, location);
        }

        [Route("create-location")]
        [HttpPost]
        public async Task<IActionResult> CreateLocation( Guid businessId,[FromBody] LocationDTO locationDTO)
        {
            var result = await _locationService.CreateLocationAsync(businessId,locationDTO);
            return StatusCode(result.StatusCode, result);
        }

        [Route("generate-location-qr-code/{locationId}")]
        [HttpGet]
        public async Task<IActionResult> GenerateLocationQrCode(Guid locationId)
        {
            var result = await _locationService.GenerateLocationQrCodeAsync(locationId);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            var qrCodeBytes = (byte[])result.Result;
            return File(qrCodeBytes, "image/png");
        }
    }
}
