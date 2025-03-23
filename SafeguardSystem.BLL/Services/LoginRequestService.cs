using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.Common.Messages;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.Extensions;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class LoginRequestService : ILoginRequestService
{
    private readonly EmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginRequestService(IUnitOfWork unitOfWork, EmailService emailService, HttpClient httpClient)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    //Lấy danh sách request
    public async Task<ResponseDTO> GetAllRequests(int pageNumber, int pageSize)
    {
        // Validate pageNumber and pageSize
        if (pageNumber <= 0 || pageSize <= 0) return new ResponseDTO("Invalid page number or page size", 400);

        // Retrieve all login requests with pagination
        var paginatedRequests = await _unitOfWork.LoginRequests.GetAllRequestsPagingAsync(pageNumber, pageSize);
        if (paginatedRequests == null || !paginatedRequests.Any())
            return new ResponseDTO("No requests found in this list.", 200);

        // Map the login requests to DTOs
        var requestDTOs = paginatedRequests.Select(r => new LoginRequestDTO
        {
            Email = r.Email,
            DateSent = r.DateSent,
            Status = r.Status,
            Reason = r.Reason
        }).ToList();

        return new ResponseDTO("Login requests retrieved successfully", 200, true,
            new PaginatedList<LoginRequestDTO>(requestDTOs, paginatedRequests.Count, pageSize, pageNumber));
    }


    public async Task<ResponseDTO> ApprovalLoginGoogle(Guid requestId, string status, string reason)
    {
        var loginRequest = await _unitOfWork.LoginRequests.GetByGuIdAsync(requestId);
        // Check if the login request exists
        if (loginRequest == null) return new ResponseDTO(AdminApprovalMessage.Missing, 404);

        // Check if the login request has been approved before
        if (loginRequest.Status != "PENDING") return new ResponseDTO(AdminApprovalMessage.Same, 400);

        var user = await _unitOfWork.Users.GetByConditionAsync(u => u.UserId == loginRequest.UserId);
        if (user == null) return new ResponseDTO("User not found", 404);

        if (status == "Accepted")
        {
            loginRequest.Status = "ACCEPTED";
            loginRequest.Reason = reason ?? "Approved to login";
            loginRequest.DateApproved = DateTime.Now;
            user.IsLocked = false;
            user.IsActive = true;
            user.IsEmailConfirmed = true;

            // Change role to Business Partner
            var businessPartnerRole = await _unitOfWork.Roles.GetRoleIdByNameAsync("Business Partner");
            user.RoleID = businessPartnerRole.RoleId;

            // Create a new business for the user
            var business = new Business
            {
                BusinessId = Guid.NewGuid(),
                Name = user.FullName,
                IsActive = true,
                UserId = user.UserId,
                ContractExpiry = DateTime.UtcNow.AddYears(1),
                IsDeleted = false
            };
            await _unitOfWork.Businesses.CreateAsync(business);

            await _unitOfWork.SaveChangeAsync();

            await SendWelcomeEmail(user.Email);
        }
        else if (status == "Rejected")
        {
            loginRequest.Status = "REJECTED";
            loginRequest.Reason = reason ?? "Rejected to login";
            loginRequest.DateApproved = DateTime.Now;
            user.IsLocked = true;
            user.IsActive = false;
            user.IsEmailConfirmed = false;
            await _unitOfWork.SaveChangeAsync();
            await SendRejectEmail(user.Email);
        }

        return new ResponseDTO(AdminApprovalMessage.Complete, 200, true);
    }

    public async Task SendWelcomeEmail(string Email)
    {
        var emailRequest = new EmailRequest();
        emailRequest.Email = Email;
        emailRequest.Subject = "[NO-REPLY]Welcome to Safeguard System";
        emailRequest.EmailBody = _emailService.GenerateWelcomeGoogleLoginUser(Email);
        await _emailService.SendEmailAsync(emailRequest);
    }

    public async Task SendRejectEmail(string Email)
    {
        var emailRequest = new EmailRequest();
        emailRequest.Email = Email;
        emailRequest.Subject = "[NO-REPLY]Your request has been rejected";
        emailRequest.EmailBody = _emailService.GenerateRejectGoogleLoginUser(Email);
        await _emailService.SendEmailAsync(emailRequest);
    }
}