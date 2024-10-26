using HotelManagementSystem.Model.Entity.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateBooking
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public RoomStatus Status { get; set; }
        [Required(ErrorMessage = "TotalCost is required")]
        public decimal TotalCost { get; set; }
        [Required(ErrorMessage = "RoomId is required")]
        public Guid RoomId { get; set; }
        [Required(ErrorMessage = "CustomerId is required")]
        public string UserId { get; set; }
        public Guid BookingId { get; set; }
        public string? RoomName { get; set; }
    }
}


