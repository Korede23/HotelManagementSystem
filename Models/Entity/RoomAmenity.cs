namespace HotelManagementSystem.Model.Entity
{
    public class RoomAmenity : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid AmenityId { get; set; }
        public Room Room { get; set; }
        public Amenity Amenity { get; set; }
    }
}
