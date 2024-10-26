using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Models.Entity;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class UpdateProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<ImageDto> Images { get; set; } = new HashSet<ImageDto>();
    }
}
