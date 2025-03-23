using Microsoft.Extensions.Configuration;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;


namespace SafeguardSystem.BLL.Services;

public class AttendenceService : IAttendenceService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    
    public AttendenceService(IConfiguration configuration, HttpClient httpClient, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
    }
    
    // public async Task<ResponseDTO> CheckIn(Guid guardId, Guid shiftId, AttendenceDTO attendenceDTO)
    // {
    //     var securityShift = await _unitOfWork.SecurityShifts.GetAsync(ss => ss.ShiftId == shiftId);
    //     if (securityShift == null)
    //     {
    //         return new ResponseDTO("Shift not found", 404);
    //     }
    //
    //     if (attendenceDTO.CheckInDate != securityShift.ShiftDate)
    //     {
    //         return new ResponseDTO("Check-in date does not match shift date", 400);
    //     }
    //
    //     var location = await _unitOfWork.Locations.GetAsync(l => l.LocationId == securityShift.LocationId);
    //     if (location == null)
    //     {
    //         return new ResponseDTO("Location not found", 404);
    //     }
    //
    //     var distance = GetDistance(attendenceDTO.Latitude, attendenceDTO.Longitude, location.Latitude, location.Longitude);
    //     var status = distance <= 100 ? "attended" : "absent";
    //
    //     var attendance = new Attendance
    //     {
    //         AttendanceId = Guid.NewGuid(),
    //         GuardId = guardId,
    //         ShiftId = shiftId,
    //         Status = status,
    //         CheckInDate = attendenceDTO.CheckInDate,
    //         CheckInTime = attendenceDTO.CheckInTime,
    //         Latitude = attendenceDTO.Latitude,
    //         Longitude = attendenceDTO.Longitude
    //     };
    //
    //     await _unitOfWork.Attendences.AddAsync(attendance);
    //     await _unitOfWork.SaveChangeAsync();
    //
    //     return new ResponseDTO("Check-in recorded successfully", 200, true);
    // }
    //
    // private double GetDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    // {
    //     // var sCoord = new GeoCoordinate((double)lat1, (double)lon1);
    //     // var eCoord = new GeoCoordinate((double)lat2, (double)lon2);
    //     return sCoord.GetDistanceTo(eCoord);
    // }
}
