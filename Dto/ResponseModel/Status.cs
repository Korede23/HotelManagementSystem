using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Dto.ResponseModel
{
    public class Status
    {
        public string Message { get;set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public UserRole? Role { get; set; }
    }
}
