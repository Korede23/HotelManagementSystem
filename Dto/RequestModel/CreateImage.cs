using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateImage
    {
        public Guid RoomId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
