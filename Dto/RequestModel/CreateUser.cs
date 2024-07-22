﻿using HotelManagementSystem.Model.Entity.Enum;
namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateUser
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Password { get; set; }
        public Gender Gender { get; set; }
        public UserRole UserRole { get; set; }
    }
}
