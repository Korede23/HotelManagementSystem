using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateRoom
    {
        // public int Id { get; set; }
        [Required(ErrorMessage = "RoomName is required")]
        public string RoomName { get; set; }
        [Required(ErrorMessage = "RoomNumber is required")]
        public int RoomNumber { get; set; }
        [Required(ErrorMessage = "RoomType is required")]
        public RoomType RoomType { get; set; }
        [Required(ErrorMessage = "BedType is required")]
        public BedType BedType { get; set; }
        [Required(ErrorMessage = "MaxOccupancy is required")]
        public int MaxOccupancy { get; set; }
        [Required(ErrorMessage = "RoomRate is required")]
        public decimal RoomRate { get; set; }
        [Required(ErrorMessage = "RoomId is required")]
        public Guid RoomId { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public Amenity Amenity { get; set; }
        public Guid AmenityId { get; set; }
        [Required(ErrorMessage = "RoomAvailability is required")]
        public RoomAvailability Availability { get; set; }
        [Required(ErrorMessage = "Please upload at least one image for the room.")]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();


    }
}
