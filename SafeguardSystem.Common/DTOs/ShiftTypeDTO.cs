namespace SafeguardSystem.Common.DTOs;

public class ShiftTypeDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string ShiftTime => $"{StartTime} - {EndTime}";
}