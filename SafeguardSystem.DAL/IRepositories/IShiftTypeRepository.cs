using SafeguardSystem.DAL.Entities;

namespace SafeguardSystem.DAL.IRepositories;

public interface IShiftTypeRepository : IGenericRepository<ShiftType>
{
    Task<List<ShiftType>> GetAllShiftTypesAsync();
    Task<ShiftType> GetShiftTypeByIdAsync(Guid id);
    Task<bool> IsTimeConflictAsync(TimeSpan startTime, TimeSpan endTime, Guid? excludeId = null);
    bool ValidateShiftTime(TimeSpan startTime, TimeSpan endTime, out string errorMessage);
}