using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Models.Entity
{
    public class CustomerStatus : BaseEntity
    {
        public Guid BookingId { get; set; }
        public string CustomerId { get; set; } 
        public string CustomerName { get; set; } 
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
