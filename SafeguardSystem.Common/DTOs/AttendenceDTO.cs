namespace SafeguardSystem.Common.DTOs;

public class AttendenceDTO
{
    public DateOnly CheckInDate { get; set; }
    public TimeOnly CheckInTime { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}