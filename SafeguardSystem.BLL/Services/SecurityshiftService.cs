using System.Drawing.Imaging;
using QRCoder;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class SecurityshiftService : ISecurityshiftService
{
    private readonly IUnitOfWork _unitOfWork;

    public SecurityshiftService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDTO> AssignShiftAsync(SecurityShiftDTO securityShiftDTO)
    {
        if (securityShiftDTO.ShiftDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return new ResponseDTO("Cannot assign a shift with a date in the past", 400);
        }

        var location = await _unitOfWork.Locations.GetLocationByIdAsync(securityShiftDTO.LocationId);
        if (location == null) return new ResponseDTO("Location not found", 404);

        var team = await _unitOfWork.Teams.GetByGuIdAsync(securityShiftDTO.TeamId);
        if (team == null) return new ResponseDTO("Team not found", 404);

        var type = await _unitOfWork.ShiftTypes.GetByGuIdAsync(securityShiftDTO.TypeId);
        if (type == null) return new ResponseDTO("Type not found", 404);

        var securityshift = new SecurityShift
        {
            ShiftId = Guid.NewGuid(),
            LocationId = securityShiftDTO.LocationId,
            TeamId = securityShiftDTO.TeamId,
            TypeId = securityShiftDTO.TypeId,
            ShiftDate = securityShiftDTO.ShiftDate
        };
        await _unitOfWork.SecurityShifts.AddAsync(securityshift);

        var teamGuards = await _unitOfWork.TeamGuards.GetAllAsync(tg => tg.TeamId == securityShiftDTO.TeamId);
        byte[] qrCodeData = null;
        foreach (var teamGuard in teamGuards)
        {
            var attendance = new Attendance
            {
                AttendanceId = Guid.NewGuid(),
                GuardId = teamGuard.GuardId,
                ShiftId = securityshift.ShiftId,
                Status = "NOT YET"
            };
            await _unitOfWork.Attendences.AddAsync(attendance);
            
            //Chỉnh lại url của api
            var checkInUrl = $"https://yourapi.com/checkin?attendanceId={attendance.AttendanceId}&latitude={attendance.Latitude}&longitude={attendance.Longitude}";
            qrCodeData = GenerateQrCode(checkInUrl);
        }
        await _unitOfWork.SaveChangeAsync();

        return new ResponseDTO("Shift assigned to team successfully", 200, true, qrCodeData);
    }


    public async Task<ResponseDTO> DeleteShiftAsync(Guid shiftId)
    {
        var shift = await _unitOfWork.SecurityShifts.GetByGuIdAsync(shiftId);
        if (shift == null) return new ResponseDTO("Shift not found", 404);

        var attendances = await _unitOfWork.Attendences.GetAllAsync(a => a.ShiftId == shiftId);
        foreach (var attendance in attendances)
        {
            _unitOfWork.Attendences.Delete(attendance);
        }
        _unitOfWork.SecurityShifts.Delete(shift);
        await _unitOfWork.SaveChangeAsync();

        return new ResponseDTO("Shift and related attendances deleted successfully", 200, true);
    }
    
    public async Task<ResponseDTO> GetShiftsByGuardIdAsync(Guid guardId)
    {
        var attendances = await _unitOfWork.Attendences.GetShiftsByGuardIdAsync(guardId);
        if (!attendances.Any())
        {
            return new ResponseDTO("No shifts found for the guard", 404);
        }
        return new ResponseDTO("Shifts retrieved successfully", 200, true, attendances);
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