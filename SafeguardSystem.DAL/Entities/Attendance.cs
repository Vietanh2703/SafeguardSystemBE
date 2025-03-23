using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeguardSystem.DAL.Entities;

public class Attendance
{
    [Key]
    [Required(ErrorMessage = "Id is required")]
    public Guid AttendanceId { get; set; }

    [Required(ErrorMessage = "Security Shift is required")]
    public Guid ShiftId { get; set; }

    [Required(ErrorMessage = "Guard is required")]
    public Guid GuardId { get; set; }

    [Required(ErrorMessage = "Check in date is required")]
    public DateOnly CheckInDate { get; set; }

    [Required(ErrorMessage = "Check in time is required")]
    public TimeOnly CheckInTime { get; set; }

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Status { get; set; }

    [ForeignKey("ShiftId")] public virtual SecurityShift Shift { get; set; }

    [ForeignKey("GuardId")] public virtual SecurityGuard Guard { get; set; }
}