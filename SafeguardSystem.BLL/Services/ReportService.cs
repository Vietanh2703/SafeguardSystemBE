using Microsoft.AspNetCore.Http;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAWSS3Service _awsS3Service;
        private const long MaxFileSize = 20 * 1024 * 1024; //Max file size is 20MB

        private static readonly List<string> AllowedFileTypes = new List<string>
        {
            // Image file types
            "image/jpeg", "image/png", "image/gif",

            // Video file types
            "video/mp4", "video/avi", "video/mpeg"
        };

        public ReportService(IUnitOfWork unitOfWork, IAWSS3Service awsS3Service)
        {
            _unitOfWork = unitOfWork;
            _awsS3Service = awsS3Service;
        }

        public async Task<ResponseDTO> GetAllReportsAsync()
        {
            try
            {
                var results = await _unitOfWork.Reports.GetAllReportsAsync();
                if (results == null || !results.Any())
                {
                    return new ResponseDTO("Empty report list", 200, false);
                }

                var reportDTOs = results.Select(report => new ReportDTO
                {
                    ReportComment = report.ReportComment,
                    Sender = report.Sender,
                    ImageUrl = report.ImageUrl,
                    CreatedAt = report.CreatedAt
                }).ToList();

                return new ResponseDTO("Reports retrieved successfully", 200, true, reportDTOs);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false);
            }
        }

        public async Task<ResponseDTO> CreateReportAsync(string userId, CreateReportDTO reportDTO)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByFirebaseUidAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    return new ResponseDTO("Cannot find user.", 400, false);
                }

                // Verify the file
                if (reportDTO.ImageFile != null)
                {
                    var fileVerificationResult = FileVerification(reportDTO.ImageFile);
                    if (!fileVerificationResult.IsSuccess)
                    {
                        return fileVerificationResult;
                    }
                }

                // Upload image to AWS S3
                string imageUrl = null;
                if (reportDTO.ImageFile != null)
                {
                    var uploadResult = await _awsS3Service.DefaultUploadFileAsync(reportDTO.ImageFile, userId);
                    if (!uploadResult.IsSuccess)
                    {
                        return new ResponseDTO($"Failed to upload image: {uploadResult.Message}", 500, false);
                    }
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
                    report.CreatedAt,
                });
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false);
            }
        }


        private ResponseDTO FileVerification(IFormFile file)
        {
            ResponseDTO result = new ResponseDTO("StepAttachment not found", 404, false);

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
}
