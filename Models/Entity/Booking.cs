using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Model.Entity
{
    public class Booking : BaseEntity
    {
        public DateTime CheckIn { get; set; }
        public DateTime Checkout { get; set; }
        public RoomStatus Status { get; set; }
        public Guid RoomId { get; set; }
        public decimal TotalCost { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
