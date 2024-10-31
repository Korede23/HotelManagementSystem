using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Dto
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime DateRequested { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
