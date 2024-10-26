using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Model.Entity
{
    public class Booking : BaseEntity
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalCost { get; set; }
        public string CustomerId { get; set; }
        public Guid RoomId { get; set; }
        public Room Rooms { get; set; } 
        public User Customer { get; set; } 
    }

}
