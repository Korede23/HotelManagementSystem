using HotelManagementSystem.Model.Entity.Enum;
using Microsoft.AspNetCore.Identity;

namespace HotelManagementSystem.Model.Entity
{
    public class User : IdentityUser
    {
        public UserRole UserRole { get; set; } 
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string? Name { get; set; } 
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string AgeRange { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
