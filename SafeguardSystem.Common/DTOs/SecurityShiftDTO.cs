namespace SafeguardSystem.Common.DTOs;

public class SecurityShiftDTO
{
    public Guid LocationId { get; set; }
    public string? LocationName { get; set; }
    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }
    public Guid TypeId { get; set; }
    public string? TypeName { get; set; }
    public DateOnly ShiftDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}