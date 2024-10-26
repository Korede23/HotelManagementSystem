using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Models.Entity;

namespace HotelManagementSystem.Model.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<Images> Images { get; set; } = new HashSet<Images>();
        
    }
}
