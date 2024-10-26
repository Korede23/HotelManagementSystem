namespace HotelManagementSystem.Dto
{
    public class CustomerStatusDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public string CustomerName { get; set; }
        public string? CustomerId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
