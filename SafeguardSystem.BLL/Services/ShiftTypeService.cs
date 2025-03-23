using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class ShiftTypeService : IShiftTypeService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShiftTypeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDTO> GetAllShiftTypesAsync()
    {
        var shiftTypes = await _unitOfWork.ShiftTypes.GetAllShiftTypesAsync();
        if (shiftTypes == null || !shiftTypes.Any()) return new ResponseDTO("No shift type found", 200, true);
        var shiftTypeDto = shiftTypes.Select(s => new ShiftTypeDTO
        {
            Name = s.Name,
            Description = s.Description,
            StartTime = s.StartTime,
            EndTime = s.EndTime
        }).ToList();

        var formattedResult = shiftTypeDto.Select(s => new
        {
            s.Name,
            s.Description,
            ShiftTime = $"Shift time: {s.ShiftTime}"
        }).ToList();

        return new ResponseDTO("Shift types retrieved successfully", 200, true, formattedResult);
    }

    public async Task<ResponseDTO> CreateShiftTypeAsync(ShiftTypeDTO shiftTypeDTO)
    {
        try
        {
            // // Kiểm tra StartTime & EndTime hợp lệ
            if (shiftTypeDTO == null) return new ResponseDTO("Invalid shift time.", 400);

            if (shiftTypeDTO.StartTime == default || shiftTypeDTO.EndTime == default)
                return new ResponseDTO("Invalid shift time.", 400);


            var existingShifts = await _unitOfWork.ShiftTypes.GetAllShiftTypesAsync();

            if (existingShifts.Any() &&
                await _unitOfWork.ShiftTypes.IsTimeConflictAsync(shiftTypeDTO.StartTime, shiftTypeDTO.EndTime))
                return new ResponseDTO("Shift time conflicts with an existing shift.", 400);

            if (!_unitOfWork.ShiftTypes.ValidateShiftTime(shiftTypeDTO.StartTime, shiftTypeDTO.EndTime,
                    out var errorMessage)) return new ResponseDTO(errorMessage, 400);

            var shiftType = new ShiftType
            {
                TypeId = Guid.NewGuid(),
                Name = shiftTypeDTO.Name,
                Description = shiftTypeDTO.Description,
                StartTime = shiftTypeDTO.StartTime,
                EndTime = shiftTypeDTO.EndTime,
                IsDeleted = false
            };

            await _unitOfWork.ShiftTypes.AddAsync(shiftType);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Shift created successfully", 200, true);
        }
        catch (Exception ex)
        {
            return new ResponseDTO($"Error creating shift type: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDTO> UpdateShiftTypeAsync(Guid typeId, ShiftTypeDTO shiftTypeDTO)
    {
        try
        {
            var existingShift = await _unitOfWork.ShiftTypes.GetShiftTypeByIdAsync(typeId);
            if (existingShift == null || existingShift.IsDeleted) return new ResponseDTO("Shift not found", 404);

            // // Kiểm tra StartTime & EndTime hợp lệ
            if (shiftTypeDTO == null) return new ResponseDTO("Invalid shift time.", 400);

            if (shiftTypeDTO.StartTime == default || shiftTypeDTO.EndTime == default)
                return new ResponseDTO("Invalid shift time.", 400);


            var existingShifts = await _unitOfWork.ShiftTypes.GetAllShiftTypesAsync();

            if (existingShifts.Any() &&
                await _unitOfWork.ShiftTypes.IsTimeConflictAsync(shiftTypeDTO.StartTime, shiftTypeDTO.EndTime))
                return new ResponseDTO("Shift time conflicts with an existing shift.", 400);

            if (!_unitOfWork.ShiftTypes.ValidateShiftTime(shiftTypeDTO.StartTime, shiftTypeDTO.EndTime,
                    out var errorMessage)) return new ResponseDTO(errorMessage, 400);

            existingShift.Name = shiftTypeDTO.Name;
            existingShift.Description = shiftTypeDTO.Description;
            existingShift.StartTime = shiftTypeDTO.StartTime;
            existingShift.EndTime = shiftTypeDTO.EndTime;

            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Shift updated successfully", 200, true);
        }
        catch (Exception ex)
        {
            return new ResponseDTO($"Error updating shift type: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDTO> DeleteShiftTypeAsync(Guid typeId)
    {
        try
        {
            var existingShift = await _unitOfWork.ShiftTypes.GetShiftTypeByIdAsync(typeId);
            if (existingShift == null || existingShift.IsDeleted) return new ResponseDTO("Shift not found", 404);

            existingShift.IsDeleted = true;
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Shift deleted successfully", 200, true);
        }
        catch (Exception ex)
        {
            return new ResponseDTO($"Error deleting shift type: {ex.Message}", 500);
        }
    }

    private bool IsValidTimeFormat(TimeOnly time)
    {
        var timeString = time.ToString("HH:mm:ss");
        return TimeOnly.TryParseExact(timeString, "HH:mm:ss", out _);
    }
}