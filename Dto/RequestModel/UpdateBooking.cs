using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class UpdateBooking
    {
        public Guid Id { get; set; }
        //public DateTime CheckIn { get; set; }
        //public DateTime Checkout { get; set; }
        public RoomStatus Status { get; set; }
        public decimal TotalCost { get; set; }
        public Guid RoomId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
