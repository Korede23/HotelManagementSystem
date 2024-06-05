namespace HotelManagementSystem.Model.Entity
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
