using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class SelectAmenity
    {
        public Guid Id { get; set; }
        public Amenity AmenityName { get; set; }

    }
}
