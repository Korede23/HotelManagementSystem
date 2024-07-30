using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateBooking
    {
        public DateTime CheckIn { get; set; }
        public DateTime Checkout { get; set; }
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
        public Guid CustomerId { get; set; }
       

        public class RoomDto
        {
            public Guid RoomId { get; set; }
        }
    }
}


