using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Model.Entity.Enum;
namespace HotelManagementSystem.Model.Entity
{
    public class Room : BaseEntity
    {
        public string RoomName { get; set; }
        public int RoomNumber { get; set; }
        public int RoomCount { get; set; }
        public RoomType RoomType { get; set; }
        public BedType BedType { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal RoomRate { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public List<SelectAmenity> Amenity { get; set; }
        public bool Availability { get; set; } 

        public ICollection<Amenity> Amenities { get; set; }

    }
}
