﻿using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Model.Entity.Enum;
namespace HotelManagementSystem.Model.Entity
{
    public class Room : BaseEntity
    {
        public string RoomName { get; set; }
        public int RoomNumber { get; set; }
        public Guid RoomId { get; set; }
        public RoomType RoomType { get; set; }
        public BedType BedType { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal RoomRate { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public RoomAvailability Availability { get; set; }
        public Guid AmenityId { get; set; }
        public Amenity Amenity { get; set; }
    }
}
