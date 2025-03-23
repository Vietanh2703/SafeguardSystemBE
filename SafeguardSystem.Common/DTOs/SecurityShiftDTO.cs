namespace SafeguardSystem.Common.DTOs;

public class SecurityShiftDTO
{
    public Guid LocationId { get; set; }
    public Guid TeamId { get; set; }
    public Guid TypeId { get; set; }
    public DateOnly ShiftDate { get; set; }
}