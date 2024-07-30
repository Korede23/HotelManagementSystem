using HotelManagementSystem.Model.Entity.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreatePayment
    {
        [Required(ErrorMessage = "PaymentId is required")]
        public Guid PaymentId { get; set; }
        [Required(ErrorMessage = "BookingId is required")]
        public Guid BookingId { get; set; }
        [Required(ErrorMessage = "PaymentMethod is required")]
        public PaymentMethod? PaymentMethod { get; set; }
        [Required(ErrorMessage = "PaymentDate is required")]
        public DateTime PaymentDate { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Balance is required")]
        public decimal Balance { get; set; }
        [Required(ErrorMessage = "PaymentStatus is required")]
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
