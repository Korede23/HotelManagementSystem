using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Model.Entity
{

    public class Payment : BaseEntity
    {
        public Guid BookingId { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public string Status { get; set; }
        public Guid OrderId { get; set; } = Guid.Empty;
        public DateTime DateRequested { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}