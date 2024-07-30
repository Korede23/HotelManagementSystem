using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HotelManagementSystem.Implementation.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImageService _imageService;

        public RoomService(ApplicationDbContext dbContext, IImageService imageService)
        {

            _dbContext = dbContext;
            _imageService = imageService;
        }

        public async Task<BaseResponse<Guid>> CreateRoom(CreateRoom request)
        {
            try
            {
                if (request != null)
                {
                    // Check if the room already exists
                    var existingRoom = await _dbContext.Rooms.FirstOrDefaultAsync(x =>
                        x.Id == request.RoomId);

                    if (existingRoom != null)
                    {
                        // Room already exists
                        return new BaseResponse<Guid>
                        {
                            Success = true,
                            Message = $"Room {request.RoomName} already exists.",
                            Hasherror = true
                        };
                    }

                    var amenity = await _dbContext.Amenities.FirstOrDefaultAsync(a => a.Id == request.AmenityId);
                    if (amenity == null)
                    {
                        return new BaseResponse<Guid>
                        {
                            Success = false,
                            Message = $"Amenity with ID {request.AmenityId} does not exist.",
                            Hasherror = true
                        };
                    }

                    // Create a new room
                    var room = new Room
                    {
                        RoomName = request.RoomName,
                        Availability = request.Availability,
                        RoomNumber = request.RoomNumber,
                        RoomRate = request.RoomRate,
                        RoomStatus = request.RoomStatus,
                        BedType = request.BedType,
                        RoomType = request.RoomType,
                        MaxOccupancy = request.MaxOccupancy,
                        AmenityId = request.AmenityId,
                        Amenity = request.Amenity,
                    };

                    await _dbContext.Rooms.AddAsync(room);
                    var result = await _dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        await _imageService.AddImagesAsync(request.Images, room.Id);
                    }
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Room {request.RoomName} created successfully.",
                        Data = room.Id
                    };
                }

                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Invalid request."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"Room creation failed: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<Guid>> DeleteRoomAsync(Guid Id)
        {
            try
            {
                var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == Id);
                if (room == null)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = $"Room not found."
                    };
                }

                _dbContext.Rooms.Remove(room);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Room  has been deleted successfully.",
                        Data = Id
                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = $"Failed to delete room.There was an error in the deletion process."
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"An error occurred while deleting the room : {ex.Message}"
                };
            }
        }



        public async Task<BaseResponse<RoomDto>> GetRoomAsync(Guid Id)
        {
            var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == Id);
            if (room != null)
            {
                return new BaseResponse<RoomDto>
                {
                    Message = "",
                    Success = true,
                    Data = new RoomDto
                    {

                        RoomNumber = room.RoomNumber,
                        Availability = room.Availability,
                        BedType = room.BedType,
                        Id = room.Id,
                        MaxOccupancy = room.MaxOccupancy,
                        RoomRate = room.RoomRate,
                        RoomName = room.RoomName,
                        RoomStatus = room.RoomStatus,
                        RoomType = room.RoomType,
                    }

                };

            }
            return new BaseResponse<RoomDto>
            {
                Success = false,
                Message = "",
            };
        }

        public List<SelectAmenityDto> GetAmenitySelect()
        {
            var amenityName = _dbContext.Amenities.ToList();
            var result = new List<SelectAmenityDto>();

            if (amenityName.Count > 0)
            {
                result = amenityName.Select(x => new SelectAmenityDto()
                {
                    Id = x.Id,
                    AmenityName = x.AmenityName,
                }).ToList();
            }

            return result;
        }


        public async Task<List<RoomDto>> GetAllRoomsCreatedAsync()
        {
            return await _dbContext.Rooms
              .Include(x => x.Amenity)
              .Include(x => x.Images)
             .Select(x => new RoomDto()
             {
                 Id = x.Id,
                 RoomName = x.RoomName,
                 RoomNumber = x.RoomNumber,
                 Availability = x.Availability,
                 RoomRate = x.RoomRate,
                 RoomStatus = x.RoomStatus,
                 BedType = x.BedType,
                 RoomType = x.RoomType,
                 MaxOccupancy = x.MaxOccupancy,
                 AmenityName = x.Amenity.AmenityName,
                 Images = x.Images.Select(x => new Dto.ImageDto
                 {
                     Id = x.Id,
                     ImagePath = x.ImagePath,
                 }).ToList()
             }).ToListAsync();
        }


        public async Task<BaseResponse<RoomDto>> GetRoomsByIdAsync(Guid Id)
        {

            var rooms = await _dbContext.Rooms
             .Where(x => x.Id == Id)
             .Select(x => new RoomDto()
             {
                 Id = x.Id,
                 RoomName = x.RoomName,
                 RoomNumber = x.RoomNumber,
                 Availability = x.Availability,
                 RoomRate = x.RoomRate,
                 RoomStatus = x.RoomStatus,
                 BedType = x.BedType,
                 RoomType = x.RoomType,
                 MaxOccupancy = x.MaxOccupancy,
                 AmenityName = x.Amenity.AmenityName,
                 Images = x.Images.Select(x => new Dto.ImageDto
                 {
                     Id = x.Id,
                     ImagePath = x.ImagePath,
                 }).ToList()
             }).FirstOrDefaultAsync();
            if (rooms != null)
            {
                return new BaseResponse<RoomDto>
                {
                    Success = true,
                    Message = $"Room Retrieved succesfully",
                    Data = rooms

                };
            }
            else
            {
                return new BaseResponse<RoomDto>
                {
                    Success = false,
                    Message = $"Room Retrieval Failed"
                };
            }

        }



        public async Task<BaseResponse<RoomDto>> UpdateRoom(Guid Id, UpdateRoom request)
        {
            try
            {
                var room = _dbContext.Rooms.FirstOrDefault(x => x.Id == Id);
                if (room == null)
                {
                    return new BaseResponse<RoomDto>
                    {
                        Success = false,
                        Message = $"Room {request.RoomName} Update failed",
                        Hasherror = true
                    };

                }
                room.RoomNumber = request.RoomNumber;
                room.RoomName = request.RoomName;
                room.RoomRate = request.RoomRate;
                room.RoomStatus = request.RoomStatus;
                room.BedType = request.BedType;
                room.RoomType = request.RoomType;
                room.MaxOccupancy = request.MaxOccupancy;
                room.RoomStatus = request.RoomStatus;
                // room.Amenity = request.Amenity;
                _dbContext.Rooms.Update(room);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<RoomDto>
                    {
                        Success = true,
                        Message = $"Room {request.RoomName} Updated Succesfully",
                    };

                }
                else
                {
                    return new BaseResponse<RoomDto>
                    {
                        Success = false,
                        Message = $"Room {request.RoomName} Update failed",
                        Hasherror = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<RoomDto>
                {
                    Success = false,
                    Message = $"Room {request.Id} Update failed",
                    Hasherror = true
                };
            }


        }


    }
}


