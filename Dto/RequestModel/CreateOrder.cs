using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateOrder
    {
    
            public Guid CustomerId { get; set; }
            public DateTime OrderDate { get; set; }
            public Guid ProductId { get; set; }
            public decimal TotalAmount { get; set; }
            public List<ProductDto> Products { get; set; }
        

    }
}
