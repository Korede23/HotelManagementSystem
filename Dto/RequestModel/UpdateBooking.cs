using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class UpdateBooking
    {
        public Guid Id { get; set; }
       // public int Duration { get; set; }
        //public DateTime CheckIn { get; set; }
        //public DateTime Checkout { get; set; }
        public RoomStatus Status { get; set; }
        public decimal TotalCost { get; set; }
        public int RoomId { get; set; } = 0;

    }
}
