using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class RequestPasswordResetService : IRequestPasswordResetService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RequestPasswordReset> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;

        public RequestPasswordResetService(ApplicationDbContext dbContext, ILogger<RequestPasswordReset> logger,
          UserManager<User> userManager, IEmailService emailSender , IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
        }
        public async Task<BaseResponse<bool>> CreateNewPassWord(CreateNewPassWord request)
        {
            try
            {
                _logger.LogInformation("Start Create Password Request");

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogInformation($"User with the {request.Email} does not exist");
                    return new BaseResponse<bool>
                    {
                        Success = false,
                        Message = $"User with the {request.Email} does not exist"
                    };
                }

                // Generate the reset code
                string resetCode = GenerateRandomCode(8);
                var userPrincipal = _httpContextAccessor.HttpContext?.User;
                if (userPrincipal == null)
                {
                    return new BaseResponse<bool>
                    {
                        Success = false,
                        Message = "User not authenticated"
                    };
                }

                var username = await _userManager.GetUserAsync(userPrincipal);
                if (user == null)
                {
                    return new BaseResponse<bool>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }
                var passwordResetRequest = new RequestPasswordReset
                {
                    Email = request.Email,
                    ResetCode = resetCode,
                    UserId = user.Id,
                    RequestedAt = DateTime.UtcNow,
                    CreatedTime = DateTime.Now,
                    IsUsed = false,
                    CreatedBy = username.UserName

                };

                // Save to the database
                await _dbContext.RequestPasswordResets.AddAsync(passwordResetRequest);
                await _dbContext.SaveChangesAsync();

                // Prepare and send the email
                var mailRequest = new MailRequests
                {
                    Body = $"<p>Hello {user.FirstName},</p><p>Your password reset code is: <strong>{resetCode}</strong></p>",
                    Title = "Password Reset Code",
                    ToEmail = user.Email,
                    HtmlContent = $"<p>Hello {user.FirstName},</p><p>Your password reset code is: <strong>{resetCode}</strong></p>"
                };

                var emailResponse = await _emailSender.SendEmailAsync(new MailReceiverDto { Email = user.Email, Name = user.FirstName + " " + user.LastName }, mailRequest);

                if (!emailResponse)
                {
                    return new BaseResponse<bool>
                    {
                        Success = false,
                        Message = "Failed to send reset code"
                    };
                }

                _logger.LogInformation("Code Generated Successfully");
                return new BaseResponse<bool>
                {
                    Success = true,
                    Message = "Code Generated Successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the reset code");
                return new BaseResponse<bool>
                {
                    Success = false,
                    Message = "Code Creation failed"
                };
            }
        }


        public async Task<BaseResponse<bool>> ValidateResetCodeAsync(ValidateResetCodeRequest request)
        {
            try
            {
                var resetRequest = await _dbContext.RequestPasswordResets.FirstOrDefaultAsync(r => r.ResetCode == request.ResetCode && !r.IsUsed);

                if (resetRequest == null || (DateTime.UtcNow - resetRequest.RequestedAt).TotalMinutes > 5)
                {
                    return new BaseResponse<bool>
                    {
                        Success = false,
                        Message = "The code you entered has expired or is invalid."
                    };
                }

                var user = await _userManager.FindByIdAsync(resetRequest.UserId);
                if (user == null)
                {
                    return new BaseResponse<bool>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var result = await _userManager.ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), request.NewPassword);

                if (result.Succeeded)
                {
                    resetRequest.IsUsed = true;
                    resetRequest.UsedAt = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();

                    return new BaseResponse<bool>
                    {
                        Success = true,
                        Message = "Password reset successful"
                    };
                }

                return new BaseResponse<bool>
                {
                    Success = false,
                    Message = "Failed to reset password"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating the reset code");
                return new BaseResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred"
                };
            }
        }


        private string GenerateRandomCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();

            return new string(Enumerable
                .Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }

}



