using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using HotelManagementSystem.Models.Entity;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class UpdateRoom
    {
        public Guid Id { get; set; }
        public string RoomName { get; set; }
        public int RoomNumber { get; set; }
        public RoomType RoomType { get; set; }
        public BedType BedType { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal RoomRate { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public ICollection<ImageDto> Images { get; set; } = new HashSet<ImageDto>();
    }
}
