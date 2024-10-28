using HMS.Implementation.Interface;
using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Services;
using HotelManagementSystem.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace HMS.Implementation.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AmenityService> _logger;

        public AmenityService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ILogger<AmenityService> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<BaseResponse<IList<AmenityDto>>> CreateAmenity(CreateAmenityRequestModel request)
        {
            _logger.LogInformation("CreateAmenity called");

            try
            {
                if (request == null)
                {
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = false,
                        Message = "Invalid request"
                    };
                }

                var userPrincipal = _httpContextAccessor.HttpContext?.User;
                if (userPrincipal == null)
                {
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = false,
                        Message = "User not authenticated"
                    };
                }

                var user = await _userManager.GetUserAsync(userPrincipal);
                if (user == null)
                {
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var roomAmenity = new Amenity
                {
                    AmenityName = request.AmenityName,
                    CreatedBy = user.UserName
                };

                _dbContext.Amenities.Add(roomAmenity);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    var amenities = await _dbContext.Amenities
                        .Select(a => new AmenityDto
                        {
                            Id = a.Id,
                            AmenityName = a.AmenityName,
                            CreatedBy = a.CreatedBy
                        })
                        .ToListAsync();

                    _logger.LogInformation("Amenity created successfully");

                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = true,
                        Message = "Amenity created successfully",
                        Data = amenities
                    };
                }
                else
                {
                    _logger.LogWarning("Amenity creation failed");
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = false,
                        Message = "Amenity creation failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating amenity");
                return new BaseResponse<IList<AmenityDto>>
                {
                    Success = false,
                    Message = "Failed due to an exception"
                };
            }
        }

        public async Task<BaseResponse<Guid>> DeleteAmenity(Guid Id)
        {
            _logger.LogInformation("DeleteAmenity called with Id: {Id}", Id);
            try
            {
                var amenity = await _dbContext.Amenities.FirstOrDefaultAsync(x => x.Id == Id);
                if (amenity != null)
                {
                    _dbContext.Amenities.Remove(amenity);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Amenity deleted successfully");
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Amenity Deleted Successful"
                    };
                }
                else
                {
                    _logger.LogWarning("Amenity deletion failed");
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting amenity");
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Failed"
                };
            }
        }

        public async Task<List<AmenityDto>> GetAmenity()
        {
            _logger.LogInformation("GetAmenity called");
            return await _dbContext.Amenities
                .Select(x => new AmenityDto()
                {
                    Id = x.Id,
                    AmenityName = x.AmenityName,
                }).ToListAsync();
        }

        public async Task<BaseResponse<AmenityDto>> GetAmenityBYId(Guid Id)
        {
            _logger.LogInformation("GetAmenityBYId called with Id: {Id}", Id);
            try
            {
                var amenity = await _dbContext.Amenities
                            .Where(x => x.Id == Id)
                            .Select(x => new AmenityDto
                            {
                                AmenityName = x.AmenityName,
                            }).FirstOrDefaultAsync();
                if (amenity != null)
                {
                    _logger.LogInformation("Amenity retrieved successfully");
                    return new BaseResponse<AmenityDto>
                    {
                        Success = true,
                        Message = "Successful",
                        Data = amenity
                    };
                }
                else
                {
                    _logger.LogWarning("Amenity not found");
                    return new BaseResponse<AmenityDto>
                    {
                        Success = false,
                        Message = "Successful"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving amenity by Id");
                return new BaseResponse<AmenityDto>
                {
                    Success = false,
                    Message = "Successful"
                };
            }
        }

        public async Task<BaseResponse<AmenityDto>> GetAmenityAsync(Guid Id)
        {
            _logger.LogInformation("GetAmenityAsync called with Id: {Id}", Id);
            try
            {
                var roomAmenity = await _dbContext.Amenities.FirstOrDefaultAsync(x => x.Id == Id);
                if (roomAmenity != null)
                {
                    _logger.LogInformation("Amenity retrieved successfully");
                    return new BaseResponse<AmenityDto>
                    {
                        Message = "Successful",
                        Success = true,
                        Data = new AmenityDto
                        {
                            AmenityName = roomAmenity.AmenityName,
                        }
                    };
                }
                else
                {
                    _logger.LogWarning("Amenity not found");
                    return new BaseResponse<AmenityDto>
                    {
                        Success = false,
                        Message = "Amenity not found",
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving amenity by Id");
                return new BaseResponse<AmenityDto>
                {
                    Success = false,
                    Message = "Failed to retrieve amenity",
                };
            }
        }

        public async Task<BaseResponse<IList<AmenityDto>>> GetAllAmenity()
        {
            _logger.LogInformation("GetAllAmenity called");
            try
            {
                var amenity = await _dbContext.Amenities
                           .Select(x => new AmenityDto
                           {
                               AmenityName = x.AmenityName,
                           }).ToListAsync();
                if (amenity != null)
                {
                    _logger.LogInformation("All amenities retrieved successfully");
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = true,
                        Message = "Successful",
                        Data = amenity
                    };
                }
                else
                {
                    _logger.LogWarning("No amenities found");
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = false,
                        Message = "No amenities found"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving all amenities");
                return new BaseResponse<IList<AmenityDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve amenities"
                };
            }
        }

        public async Task<BaseResponse<AmenityDto>> UpdateAmenity(Guid Id, UpdateAmenity request)
        {
            _logger.LogInformation("UpdateAmenity called with Id: {Id}", Id);
            try
            {
                var amenity = await _dbContext.Amenities.FirstOrDefaultAsync(x => x.Id == Id);
                if (amenity == null)
                {
                    _logger.LogWarning("Amenity not found for update");
                    return new BaseResponse<AmenityDto>
                    {
                        Success = false,
                        Message = "Update Failed"
                    };
                }

                amenity.AmenityName = request.AmenityName;
                _dbContext.Amenities.Update(amenity);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Amenity updated successfully");
                    return new BaseResponse<AmenityDto>
                    {
                        Success = true,
                        Message = "Updated Successful"
                    };
                }
                else
                {
                    _logger.LogWarning("Amenity update failed");
                    return new BaseResponse<AmenityDto>
                    {
                        Success = false,
                        Message = "Update Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating amenity");
                return new BaseResponse<AmenityDto>
                {
                    Success = false,
                    Message = "Update Failed"
                };
            }
        }
    }
}
