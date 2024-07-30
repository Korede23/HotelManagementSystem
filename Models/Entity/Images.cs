using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Models.Entity
{
    public class Images
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string ImagePath { get; set; }
        public Room Room { get; set; }
    }
}
