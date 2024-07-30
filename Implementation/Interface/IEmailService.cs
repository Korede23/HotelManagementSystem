using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Model.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IEmailService
    {
        Task SendEmailClient(string msg, string title, string email);
        Task<BaseResponse<MailReceiverDto>> SendMessageToUserAsync(CreateUser profile);
        Task<bool> SendEmailAsync(MailReceiverDto model, MailRequests request);
    }
}
