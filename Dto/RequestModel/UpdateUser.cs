

using HotelManagementSystem.Model.Entity.Enum;
using Microsoft.VisualBasic;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class UpdateUser
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string AgeRange { get; set; }
        public string? Password { get; set; }
        public Gender Gender { get; set; }
    }
}
