using GeoCoordinatePortable;
using Microsoft.Extensions.Configuration;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
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

    public async Task<ResponseDTO> CheckInAsync(Guid attendenceId, AttendenceDTO attendenceDTO)
{
    var attendance = await _unitOfWork.Attendences.GetAsync(a => a.AttendanceId == attendenceId);
    if (attendance == null)
    {
        return new ResponseDTO("Attendance record not found", 404);
    }

    var securityShift = await _unitOfWork.SecurityShifts.GetAsync(ss => ss.ShiftId == attendance.ShiftId);
    if (securityShift == null)
    {
        return new ResponseDTO("Shift not found", 404);
    }

    if (attendenceDTO.CheckInDate != securityShift.ShiftDate)
    {
        return new ResponseDTO("Check-in date does not match shift date", 400);
    }

    var shiftType = await _unitOfWork.ShiftTypes.GetAsync(st => st.TypeId == securityShift.TypeId);
    if (shiftType == null)
    {
        return new ResponseDTO("Shift type not found", 404);
    }

    if (attendenceDTO.CheckInTime < TimeOnly.FromTimeSpan(shiftType.StartTime))
    {
        return new ResponseDTO("Shift has not started yet", 400);
    }

    if (attendenceDTO.CheckInTime > TimeOnly.FromTimeSpan(shiftType.EndTime))
    {
        attendance.Status = "ABSENT";
    }
    
    else
    {
        var location = await _unitOfWork.Locations.GetAsync(l => l.LocationId == securityShift.LocationId);
        if (location == null)
        {
            return new ResponseDTO("Location not found", 404);
        }

        var distance = GetDistance(attendenceDTO.Latitude, attendenceDTO.Longitude, location.Latitude, location.Longitude);
        attendance.Status = distance <= 100 ? "ATTENDED" : "ABSENT";
    }

    attendance.CheckInDate = attendenceDTO.CheckInDate;
    attendance.CheckInTime = attendenceDTO.CheckInTime;
    attendance.Latitude = attendenceDTO.Latitude;
    attendance.Longitude = attendenceDTO.Longitude;

    await _unitOfWork.SaveChangeAsync();

    return new ResponseDTO("Check-in recorded successfully", 200, true);
    }
    
    public async Task<ResponseDTO> UpdateAbsentAttendancesAsync()
    {
        var attendances = await _unitOfWork.Attendences.GetAllAsync(a => a.Latitude == 0.00m && a.Longitude == 0.00m && a.CheckInTime == TimeOnly.MinValue);
    
        foreach (var attendance in attendances)
        {
            attendance.Status = "ABSENT";
        }

        await _unitOfWork.SaveChangeAsync();

        return new ResponseDTO("Attendances updated successfully", 200, true);
    }

    private double GetDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        var sCoord = new GeoCoordinate((double)lat1, (double)lon1);
        var eCoord = new GeoCoordinate((double)lat2, (double)lon2);
        return sCoord.GetDistanceTo(eCoord);
    }
}
