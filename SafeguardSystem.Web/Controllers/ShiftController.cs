using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers;

public class ShiftController : ControllerBase
{
    private readonly ISecurityshiftService _securityshiftService;
    private readonly IShiftTypeService _shiftTypeService;
    private readonly IAttendenceService _attendenceService;

    public ShiftController(IShiftTypeService shiftTypeService, ISecurityshiftService securityshiftService, IAttendenceService attendenceService)
    {
        _shiftTypeService = shiftTypeService;
        _securityshiftService = securityshiftService;
        _attendenceService = attendenceService;
    }

    /// <summary>
    ///     Security manager gets all shift type.
    /// </summary>
    [Route("shift-types")]
    [HttpGet]
    public async Task<IActionResult> GetShiftTypes()
    {
        var shiftTypes = await _shiftTypeService.GetAllShiftTypesAsync();
        return StatusCode(shiftTypes.StatusCode, shiftTypes);
    }

    /// <summary>
    ///     Security manager creates a new shift type.
    /// </summary>
    /// <param name="shiftTypeDTO">
    ///     Lưu ý khi nhập thông tin time vào nhớ định dạng hh:mm:ss
    ///     (12:30:05 hay 05:45:12) chứ nhập 12:30 hay 5:45:12 hay 24:00:00 thay vì 00:00:00 lỗi ráng chịu
    /// </param>
    [Route("shift-type")]
    [HttpPost]
    public async Task<IActionResult> CreateShiftType([FromBody] ShiftTypeDTO shiftTypeDTO)
    {
        var result = await _shiftTypeService.CreateShiftTypeAsync(shiftTypeDTO);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    ///     Security manager updates a shift type.
    /// </summary>
    /// <param name="shiftTypeDTO">
    ///     Lưu ý khi nhập thông tin time vào nhớ định dạng hh:mm:ss
    ///     (12:30:05 hay 05:45:12) chứ nhập 12:30 hay 5:45:12 hay 24:00:00 thay vì 00:00:00 lỗi ráng chịu
    /// </param>
    [Route("shift-type/{typeId}")]
    [HttpPut]
    public async Task<IActionResult> UpdateShiftType(Guid typeId, [FromBody] ShiftTypeDTO shiftTypeDTO)
    {
        var result = await _shiftTypeService.UpdateShiftTypeAsync(typeId, shiftTypeDTO);
        return StatusCode(result.StatusCode, result);
    }

    [Route("shift-type/delete/{typeId}")]
    [HttpPut]
    [SwaggerOperation(Summary = "Delete shift type", Description = "Deletes a shift type.")]
    [SwaggerResponse(200, "Shift type deleted successfully", typeof(ResponseDTO))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(404, "Shift type not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> DeleteShiftType(Guid typeId)
    {
        var result = await _shiftTypeService.DeleteShiftTypeAsync(typeId);
        return StatusCode(result.StatusCode, result);
    }

    [Route("shift")]
    [HttpPost]
    [SwaggerOperation(Summary = "Assign shift", Description = "Assigns a shift to a team at a specific location.")]
    [SwaggerResponse(200, "Shift assigned successfully", typeof(ResponseDTO))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(404, "Location or team not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> AssignShift([FromBody] SecurityShiftDTO securityShiftDTO)
    {
        var result = await _securityshiftService.AssignShiftAsync(securityShiftDTO);
        if(!result.IsSuccess) return StatusCode(result.StatusCode, result);
        var qrCodeBytes = (byte[])result.Result;
        return File(qrCodeBytes, "image/png");
    }


    [Route("shift/{shiftId}")]
    [HttpDelete]
    [SwaggerOperation(Summary = "Delete shift", Description = "Deletes a shift.")]
    [SwaggerResponse(200, "Shift deleted successfully", typeof(ResponseDTO))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(404, "Shift not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> DeleteShift([FromRoute] Guid shiftId)
    {
        var response = await _securityshiftService.DeleteShiftAsync(shiftId);
        return StatusCode(response.StatusCode, response);
    }
    
    [Route("guard/{guardId}/shifts")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get shifts by guard ID", Description = "Retrieves the list of shifts for a specific guard.")]
    [SwaggerResponse(200, "Shifts retrieved successfully", typeof(ResponseDTO))]
    [SwaggerResponse(404, "No shifts found for the guard")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetShiftsByGuardId([FromRoute] Guid guardId)
    {
        var response = await _securityshiftService.GetShiftsByGuardIdAsync(guardId);
        return StatusCode(response.StatusCode, response);
    }

    [Route("checkin")]
    [HttpPut]
    [SwaggerOperation(Summary = "Check in", Description = "Checks in a guard for a shift.")]
    [SwaggerResponse(200, "Checked in successfully", typeof(ResponseDTO))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(404, "Shift not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> CheckIn(Guid attendanceId, decimal latitude, decimal longitude)
    {
        var currentTime = DateTime.Now;
        var checkInTime = TimeOnly.FromDateTime(currentTime);

        // In ra giá trị checkInTime
        Console.WriteLine($"CheckInTime: {checkInTime}");

        var attendenceDTO = new AttendenceDTO
        {
            Latitude = latitude,
            Longitude = longitude,
            CheckInDate = DateOnly.FromDateTime(currentTime),
            CheckInTime = checkInTime
        };

        var response = await _attendenceService.CheckInAsync(attendanceId, attendenceDTO);
        return StatusCode(response.StatusCode, response);
    }

    [Route("status")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update status", Description = "Updates the status of absent attendances.")]
    [SwaggerResponse(200, "Status updated successfully", typeof(ResponseDTO))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> UpdateStatus()
    {
        var response = await _attendenceService.UpdateAbsentAttendancesAsync();
        if (response.StatusCode == 200)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}