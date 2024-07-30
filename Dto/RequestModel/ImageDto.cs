using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string ImagePath { get; set; }
        public Room Room { get; set; }
    }
}
