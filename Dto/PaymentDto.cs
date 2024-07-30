using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Dto
{
    public class PaymentDto
    {
        public Guid PaymentId { get; set; }
        public Guid BookingId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
