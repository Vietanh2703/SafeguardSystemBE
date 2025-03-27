using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IReportService
{
    Task<ResponseDTO> GetAllReportsAsync();
    Task<ResponseDTO> CreateReportAsync(string userId, CreateReportDTO reportDTO);
    Task<ResponseDTO> ResponseReportServiceAsync(Guid ReportId, ResReportDTO resReportDTO);
    Task<ResponseDTO> DeleteReportAsync(Guid reportId);
}