using Microsoft.AspNetCore.Http;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class ReportService : IReportService
{
    private const long MaxFileSize = 20 * 1024 * 1024; //Max file size is 20MB

    private static readonly List<string> AllowedFileTypes = new()
    {
        // Image file types
        "image/jpeg", "image/png", "image/gif",

        // Video file types
        "video/mp4", "video/avi", "video/mpeg"
    };

    private readonly IAWSS3Service _awsS3Service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ReportService(IUnitOfWork unitOfWork, IAWSS3Service awsS3Service, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _awsS3Service = awsS3Service;
        _emailService = emailService;
    }

    public async Task<ResponseDTO> GetAllReportsAsync()
    {
        try
        {
            var results = await _unitOfWork.Reports.GetAllReportsAsync();
            if (results == null || !results.Any()) return new ResponseDTO("Empty report list", 200);

            var reportDTOs = results.Select(report => new ReportDTO
            {
                ReportId = report.ReportId,
                ReportComment = report.ReportComment,
                Sender = report.Sender,
                ImageUrl = report.ImageUrl,
                Status = report.Status,
                CreatedAt = report.CreatedAt
            }).ToList();

            return new ResponseDTO("Reports retrieved successfully", 200, true, reportDTOs);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(ex.Message, 500);
        }
    }

    public async Task<ResponseDTO> CreateReportAsync(string userId, CreateReportDTO reportDTO)
    {
        try
        {
            var user = await _unitOfWork.Users.GetUserWithRoleByFirebaseUidAsync(userId);
            if (user == null || user.IsDeleted) return new ResponseDTO("Cannot find user.", 400);

            // Verify the file
            if (reportDTO.ImageFile != null)
            {
                var fileVerificationResult = FileVerification(reportDTO.ImageFile);
                if (!fileVerificationResult.IsSuccess) return fileVerificationResult;
            }

            // Upload image to AWS S3
            string imageUrl = null;
            if (reportDTO.ImageFile != null)
            {
                var uploadResult = await _awsS3Service.DefaultUploadFileAsync(reportDTO.ImageFile, userId);
                if (!uploadResult.IsSuccess)
                    return new ResponseDTO($"Failed to upload image: {uploadResult.Message}", 500);
                imageUrl = uploadResult.Result.ToString();
            }

            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                Sender = user.FullName,
                RoleName = user.Role.RoleName,
                Respondent = "N/A",
                ReportComment = reportDTO.ReportComment,
                Reason = "null",
                IsClosed = false,
                ImageUrl = imageUrl,
                Status = "PENDING",
                AnsweredAt = DateTime.MinValue,
                CreatedAt = DateTime.UtcNow,
                UserId = user.UserId
            };

            await _unitOfWork.Reports.AddAsync(report);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Report created successfully", 200, true, new
            {
                report.Sender,
                report.RoleName,
                report.ReportComment,
                report.ImageUrl,
                report.Status,
                report.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return new ResponseDTO(ex.Message, 500);
        }
    }

    public async Task<ResponseDTO> ResponseReportServiceAsync(Guid ReportId, ResReportDTO resReportDTO)
    {
        try
        {
            var report = await _unitOfWork.Reports.GetReportByIdAsync(ReportId);
            if (report == null) return new ResponseDTO("Report not found.", 404);

            var user = await _unitOfWork.Users.GetUserWithRoleByFirebaseUidAsync(report.UserId);
            if (user == null || user.IsDeleted) return new ResponseDTO("Cannot find user.", 400);

            report.Respondent = resReportDTO.Respondent;
            report.Reason = resReportDTO.Reason;
            report.Status = resReportDTO.Status;
            report.AnsweredAt = DateTime.UtcNow;
            report.IsClosed = true;

            await _unitOfWork.SaveChangeAsync();

            if (resReportDTO.Status == "APPROVED")
            {
                await SendApprovedEmail(user.Email, report.Sender, resReportDTO.Respondent, resReportDTO.Reason, DateTime.UtcNow);
            }
            else if (resReportDTO.Status == "REJECTED")
            {
                await SendRejectedEmail(user.Email, report.Sender, resReportDTO.Respondent, resReportDTO.Reason, DateTime.UtcNow);
            }
            else
            {
                return new ResponseDTO("Invalid status.", 400);
            }
            return new ResponseDTO("Report response processed successfully", 200, true);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(ex.Message, 500);
        }
    }

    public async Task<ResponseDTO> DeleteReportAsync(Guid reportId)
    {
        try
        {
            var report = await _unitOfWork.Reports.GetReportByIdAsync(reportId);
            if (report == null) return new ResponseDTO("Report not found.", 404);

            _unitOfWork.Reports.Delete(report);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Report deleted successfully", 200, true);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(ex.Message, 500);
        }
    }

    public async Task SendApprovedEmail(string Email,string sender, string respondent, string reason, DateTime date)
    {
        var emailRequest = new EmailRequest();
        emailRequest.Email = Email;
        emailRequest.Subject = "[NO-REPLY] Your report has been executed";
        emailRequest.EmailBody = _emailService.GenerateAcceptedReportEmail(sender,respondent,reason,date);
        await _emailService.SendEmailAsync(emailRequest);
    }

    public async Task SendRejectedEmail(string Email,string sender, string respondent, string reason, DateTime date)
    {
        var emailRequest = new EmailRequest();
        emailRequest.Email = Email;
        emailRequest.Subject = "[NO-REPLY] Your report has been executed";
        emailRequest.EmailBody = _emailService.GenerateRejectedReportEmail(sender, respondent, reason, date);
        await _emailService.SendEmailAsync(emailRequest);
    }

    private ResponseDTO FileVerification(IFormFile file)
    {
        var result = new ResponseDTO("StepAttachment not found", 404);

        if (file.Length > MaxFileSize)
        {
            result.StatusCode = 413;
            result.Message = $"File size exceeds the maximum allowed size of {MaxFileSize / (1024 * 1024)} MB.";
            result.IsSuccess = false;
            return result;
        }

        if (!AllowedFileTypes.Contains(file.ContentType))
        {
            result.StatusCode = 415;
            result.Message = "File type is not allowed. Only image and video files are allowed.";
            result.IsSuccess = false;
            return result;
        }

        return new ResponseDTO("File verification successful", 200, true);
    }
}