using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateRoom
    {
        // public int Id { get; set; }
        public string RoomName { get; set; }
        public int RoomNumber { get; set; }
        public RoomType RoomType { get; set; }
        public BedType BedType { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal RoomRate { get; set; }
        public Guid RoomId { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public Amenity Amenity { get; set; }
        public int AmenityId { get; set; }
        public bool Availability { get; set; }


    }
}
