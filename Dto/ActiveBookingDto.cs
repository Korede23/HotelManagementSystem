namespace HotelManagementSystem.Dto
{
    public class ActiveBookingDto
    {
        public Guid BookingId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
