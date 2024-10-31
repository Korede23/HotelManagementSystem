using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Model.Entity.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HotelManagementSystem.Implementation.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RoomService> _logger;

        public RoomService(ApplicationDbContext dbContext, IImageService imageService, IHttpContextAccessor httpContextAccessor,
         UserManager<User> userManager, ILogger<RoomService> logger)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> CreateRoom(CreateRoom request)
        {
            _logger.LogInformation("Start Creating new room: {RoomName}", request.RoomName);

            try
            {
                if (request != null)
                {

                    var existingRoom = await _dbContext.Rooms.AnyAsync(x => x.RoomName == request.RoomName);

                    if (existingRoom)
                    {
                        _logger.LogWarning("Room already exists: {RoomName}", request.RoomName);
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
                        _logger.LogWarning("Amenity with ID {AmenityId} does not exist", request.AmenityId);
                        return new BaseResponse<Guid>
                        {
                            Success = false,
                            Message = $"Amenity with ID {request.AmenityId} does not exist.",
                            Hasherror = true
                        };
                    }
                    var userPrincipal = _httpContextAccessor.HttpContext?.User;
                    if (userPrincipal == null)
                    {
                        return new BaseResponse<Guid>
                        {
                            Success = false,
                            Message = "User not authenticated"
                        };
                    }

                    var user = await _userManager.GetUserAsync(userPrincipal);
                    if (user == null)
                    {
                        return new BaseResponse<Guid>
                        {
                            Success = false,
                            Message = "User not found"
                        };
                    }


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
                        CreatedBy = user.UserName
                    };

                    await _dbContext.Rooms.AddAsync(room);
                    var result = await _dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        await _imageService.AddImagesAsync(request.Images, room.Id);
                    }

                    _logger.LogInformation("Room {RoomName} created successfully", request.RoomName);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Room {request.RoomName} created successfully.",
                        Data = room.Id
                    };
                }

                _logger.LogWarning("Invalid request to create room");
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Invalid request."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Room creation failed: {RoomName}", request.RoomName);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"Room creation failed: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<Guid>> DeleteRoomAsync(Guid Id)
        {
            _logger.LogInformation("Deleting room with ID: {RoomId}", Id);

            try
            {
                var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == Id);
                if (room == null)
                {
                    _logger.LogWarning("Room not found with ID: {RoomId}", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = $"Room not found."
                    };
                }

                _dbContext.Rooms.Remove(room);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Room with ID {RoomId} deleted successfully", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = $"Room has been deleted successfully.",
                        Data = Id
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to delete room with ID: {RoomId}", Id);
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = $"Failed to delete room. There was an error in the deletion process."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the room with ID: {RoomId}", Id);
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = $"An error occurred while deleting the room: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<RoomDto>> GetRoomAsync(Guid Id)
        {
            _logger.LogInformation("Retrieving room with ID: {RoomId}", Id);

            try
            {
                var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == Id);
                if (room != null)
                {
                    _logger.LogInformation("Room with ID {RoomId} retrieved successfully", Id);
                    return new BaseResponse<RoomDto>
                    {
                        Success = true,
                        Message = "",
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

                _logger.LogWarning("Room not found with ID: {RoomId}", Id);
                return new BaseResponse<RoomDto>
                {
                    Success = false,
                    Message = "",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the room with ID: {RoomId}", Id);
                return new BaseResponse<RoomDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public List<SelectAmenityDto> GetAmenitySelect()
        {
            _logger.LogInformation("Retrieving list of amenities");

            try
            {
                var amenityName = _dbContext.Amenities.ToList();
                var result = new List<SelectAmenityDto>();

                if (amenityName.Count > 0)
                {
                    result = amenityName.Select(x => new SelectAmenityDto
                    {
                        Id = x.Id,
                        AmenityName = x.AmenityName,
                    }).ToList();
                }

                _logger.LogInformation("List of amenities retrieved successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving amenities");
                return new List<SelectAmenityDto>();
            }
        }

        public async Task<PaginatedResponse<List<RoomDto>>> GetRoomsCreatedAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Retrieving all rooms with pagination");

            try
            {
                var totalRoomsCount = await _dbContext.Rooms.CountAsync();

                var rooms = await _dbContext.Rooms
                    .Include(x => x.Amenity)
                    .Include(x => x.Images)
                    .OrderByDescending(x => x.CreatedTime)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new RoomDto
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
                        Images = x.Images.Select(i => new Dto.ImageDto
                        {
                            Id = i.Id,
                            ImagePath = i.ImagePath,
                        }).ToList()
                    }).ToListAsync();

                _logger.LogInformation("All rooms retrieved successfully");

                return new PaginatedResponse<List<RoomDto>>
                {
                    Data = rooms,
                    TotalRecords = totalRoomsCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all rooms");
                return new PaginatedResponse<List<RoomDto>>
                {
                    Data = new List<RoomDto>(),
                    TotalRecords = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };
            }
        }


        public async Task<BaseResponse<RoomDto>> GetRoomsByIdAsync(Guid Id)
        {
            _logger.LogInformation("Retrieving room by ID: {RoomId}", Id);

            try
            {
                var rooms = await _dbContext.Rooms
                    .Where(x => x.Id == Id)
                    .Select(x => new RoomDto
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
                    _logger.LogInformation("Room with ID {RoomId} retrieved successfully", Id);
                    return new BaseResponse<RoomDto>
                    {
                        Success = true,
                        Message = $"Room Retrieved successfully",
                        Data = rooms
                    };
                }

                _logger.LogWarning("Room not found with ID: {RoomId}", Id);
                return new BaseResponse<RoomDto>
                {
                    Success = false,
                    Message = "Room Retrieval Failed"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the room by ID: {RoomId}", Id);
                return new BaseResponse<RoomDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<PaginatedResponse<List<RoomDto>>> GetAllRoomsCreatedAsync(
                                                        int pageNumber,
                                                        int pageSize,
                                                        string searchAmenity,
                                                        string available,
                                                        decimal roomRate,
                                                        string searchTerm = null)
        {
            _logger.LogInformation("Retrieving all rooms with pagination");

            try
            {
                IQueryable<Room> query = _dbContext.Rooms.Include(x => x.Amenity).Include(x => x.Images);
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(x =>
                        x.RoomName.Contains(searchTerm) ||
                        x.Amenity.AmenityName.Contains(searchTerm) ||
                        x.RoomNumber.Equals(searchTerm));

                }

                if (roomRate > 0)
                {
                    query = query.Where(x => x.RoomRate == roomRate);
                }
                if (Enum.TryParse(typeof(RoomAvailability), available, true, out var parsedAvailability))
                {
                    query = query.Where(x => x.Availability == (RoomAvailability)parsedAvailability);
                }
                if (!string.IsNullOrWhiteSpace(searchAmenity))
                {
                    query = query.Where(x => x.Amenity.AmenityName.Equals(searchAmenity, StringComparison.OrdinalIgnoreCase));
                }
                var totalRecords = await query.CountAsync();
                if (totalRecords == 0)
                {
                    _logger.LogInformation("No rooms found matching the search criteria.");
                    return new PaginatedResponse<List<RoomDto>>
                    {
                        Success = true,
                        Message = "No rooms found matching the search criteria.",
                        Data = new List<RoomDto>(),
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalRecords = 0
                    };
                }
                var rooms = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new RoomDto
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
                        Images = x.Images.Select(img => new Dto.ImageDto
                        {
                            Id = img.Id,
                            ImagePath = img.ImagePath,
                        }).ToList()
                    })
                    .ToListAsync();

                _logger.LogInformation("Rooms retrieved successfully with pagination");

                return new PaginatedResponse<List<RoomDto>>
                {
                    Success = true,
                    Message = "Rooms retrieved successfully.",
                    Data = rooms,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving rooms with pagination");
                return new PaginatedResponse<List<RoomDto>>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public async Task<BaseResponse<RoomDto>> UpdateRoom(Guid Id, UpdateRoom request)
        {
            _logger.LogInformation("Updating room with ID: {RoomId}", Id);

            try
            {
                var room = _dbContext.Rooms.FirstOrDefault(x => x.Id == Id);
                if (room == null)
                {
                    _logger.LogWarning("Room not found with ID: {RoomId}", Id);
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
                    _logger.LogInformation("Room with ID {RoomId} updated successfully", Id);
                    return new BaseResponse<RoomDto>
                    {
                        Success = true,
                        Message = $"Room {request.RoomName} Updated Successfully",
                    };
                }
                else
                {
                    _logger.LogWarning("Failed to update room with ID: {RoomId}", Id);
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
                _logger.LogError(ex, "An error occurred while updating the room with ID: {RoomId}", Id);
                return new BaseResponse<RoomDto>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Hasherror = true
                };
            }
        }
    }
}
