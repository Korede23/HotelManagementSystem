using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HMS.Implementation.Services
{
    public class RoomAmenityService
    {
        private readonly ApplicationDbContext _dbContext;

        public RoomAmenityService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<IList<RoomAmenityDto>>> CreateRoomAmenity(CreateRoomAmenity request)
        {
            if (request != null)
            {
                var roomAmenity = new RoomAmenity
                {
                    AmenityId = request.AmenityId,
                    RoomId = request.RoomId,
                    Amenity = request.Amenity,
                    Room = request.Room
                };
                _dbContext.RoomAmenities.Add(roomAmenity);
            }
            
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return new BaseResponse<IList<RoomAmenityDto>>
                {
                    Success = true,
                    Message = "Room Amenity Created Succesfully"
                };
            }
            else
            {
                return new BaseResponse<IList<RoomAmenityDto>>
                {

                };
            }

        }


        public async Task<BaseResponse<IList<Guid>>> DeleteRoomAmenity(Guid Id)
        {
            var roomAmenity = await _dbContext.RoomAmenities.FirstOrDefaultAsync(x => x.Id == Id);
            if (roomAmenity != null)
            {
                _dbContext.RoomAmenities.Remove(roomAmenity);
            }
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return new BaseResponse<IList<Guid>>
                {
                    Success = true,
                    Message = "Room Amenity has been deleted  "
                };
            }
            else
            {
                return new BaseResponse<IList<Guid>>    
                {
                    Success = false,
                    Message = "Failed  "
                };
            }
        }

    }
}
