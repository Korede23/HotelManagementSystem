using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateOrder
    {
        public Guid ProductId { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
