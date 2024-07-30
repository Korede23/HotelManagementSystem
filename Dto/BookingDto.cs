using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Dto
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid RoomId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Pending;
        public decimal TotalCost { get; set; }
        public string RoomName { get; set; }
        public Room Rooms { get; set; }
    }
}
