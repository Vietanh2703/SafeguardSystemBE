using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class ShiftTypeRepository : GenericRepository<ShiftType>, IShiftTypeRepository
{
    private readonly SafeguardDbContext _context;

    public ShiftTypeRepository(SafeguardDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ShiftType>> GetAllShiftTypesAsync()
    {
        return await _context.ShiftTypes.Where(st => !st.IsDeleted).ToListAsync();
    }

    public async Task<ShiftType> GetShiftTypeByIdAsync(Guid TypeId)
    {
        return await _context.ShiftTypes.FirstOrDefaultAsync(u => u.TypeId == TypeId && !u.IsDeleted);
    }

    // Kiểm tra xem có trùng lịch hay không(excludeId dùng khi sửa ca trực tránh ca trực đã sửa trùng với chính nó)
    public async Task<bool> IsTimeConflictAsync(TimeSpan startTime, TimeSpan endTime, Guid? excludeId = null)
    {
        var shifts = await _context.ShiftTypes
            .Where(s => !s.IsDeleted && (!excludeId.HasValue || s.TypeId != excludeId))
            .ToListAsync(); // Lấy danh sách về bộ nhớ

        return shifts.Any(s => IsTimeOverlap(startTime, endTime, s.StartTime, s.EndTime));
    }


    // Kiểm tra thời gian làm việc hợp lệ
    public bool ValidateShiftTime(TimeSpan startTime, TimeSpan endTime, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (startTime == null || endTime == null)
        {
            errorMessage = "Shift time cannot be null.";
            return false;
        }

        // Kiểm tra định dạng thời gian
        if (!IsValidTimeFormat(startTime) || !IsValidTimeFormat(endTime))
        {
            errorMessage = "Invalid time format. Please use HH:mm:ss.";
            return false;
        }

        // Nếu EndTime nhỏ hơn StartTime (ca qua đêm)
        if (endTime < startTime)
        {
            var durationOvernight = TimeSpan.FromHours(24) - startTime + endTime;

            if (durationOvernight > TimeSpan.FromHours(7))
            {
                errorMessage = "Overnight shift duration cannot exceed 7 hours.";
                return false;
            }

            if (durationOvernight < TimeSpan.FromHours(2))
            {
                errorMessage = "Overnight shift duration cannot be less than 2 hours.";
                return false;
            }

            return true; // Ca qua đêm hợp lệ
        }

        // Nếu EndTime lớn hơn StartTime (ca ban ngày)
        var duration = endTime - startTime;

        if (duration > TimeSpan.FromHours(7))
        {
            errorMessage = "Shift duration cannot exceed 7 hours.";
            return false;
        }

        if (duration < TimeSpan.FromHours(2))
        {
            errorMessage = "Shift duration cannot be less than 2 hours.";
            return false;
        }

        return true;
    }

    public bool IsValidTimeFormat(TimeSpan? time)
    {
        if (time == null)
            return false;

        try
        {
            var timeString = time.Value.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            return TimeOnly.TryParseExact(timeString, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out _);
        }
        catch
        {
            return false;
        }
    }

    private bool IsTimeOverlap(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
    {
        var s1 = ConvertTimeToDouble(start1);
        var e1 = ConvertTimeToDouble(end1);
        var s2 = ConvertTimeToDouble(start2);
        var e2 = ConvertTimeToDouble(end2);

        // Nếu ca nào đó qua đêm, cộng thêm 24 giờ vào phần EndTime để dễ so sánh
        if (s1 > e1) e1 += 24;
        if (s2 > e2) e2 += 24;

        // Kiểm tra overlap
        return !(e1 <= s2 || s1 >= e2);
    }


    private double ConvertTimeToDouble(TimeSpan time)
    {
        return time.Hours + time.Minutes / 60.0;
    }
}