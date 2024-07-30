using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using HotelManagementSystem.Models.Entity;

namespace HotelManagementSystem.Dto
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string RoomName { get; set; }
        public int RoomNumber { get; set; }
        public RoomType? RoomType { get; set; }
        public BedType? BedType { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal RoomRate { get; set; }
        public RoomStatus? RoomStatus { get; set; }
        public Amenity Amenities { get; set; }
        public Guid AmenityId { get; set; }
        public string AmenityName { get; set; } 
        public RoomAvailability Availability { get; set; }
        public List<ImageDto> Images { get; set; }
    }
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
    }

}



