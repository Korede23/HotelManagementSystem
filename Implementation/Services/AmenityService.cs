using HMS.Implementation.Interface;
using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HMS.Implementation.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly ApplicationDbContext _dbContext;

        public AmenityService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<IList<AmenityDto>>> CreateAmenity(CreateAmenityRequestModel request)
        {
            try
            {
                if (request != null)
                {
                    var roomAmenity = new Amenity
                    {
                        AmenityName = request.AmenityName,
                    };
                    _dbContext.Amenities.Add(roomAmenity);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = true,
                        Message = "Successful"
                    };
                }
                else
                {
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = false,
                        Message = "Failed"
                    };
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse<IList<AmenityDto>>
                {
                    Success = false,
                    Message = "Failed"
                };

            }


        }


        public async Task<BaseResponse<Guid>> DeleteAmenity(Guid Id)
        {
            try
            {
                var amenity = await _dbContext.Amenities.FirstOrDefaultAsync();
                if (amenity != null)
                {
                    _dbContext.Amenities.Remove(amenity);
                }
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<Guid>
                    {
                        Success = true,
                        Message = "Amenity Deleted Successful"
                    };
                }
                else
                {
                    return new BaseResponse<Guid>
                    {
                        Success = false,
                        Message = "Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<Guid>
                {
                    Success = false,
                    Message = "Failed"
                };
            }


        }





        public async Task<List<AmenityDto>> GetAmenity()
        {
            return _dbContext.Amenities
                //.Include(x => x.Id)
                //.Include(x => x.RoomType)
                .Select(x => new AmenityDto()
                {
                    Id = x.Id,
                    AmenityName = x.AmenityName,

                }).ToList();
        }

        public async Task<BaseResponse<AmenityDto>> GetAmenityBYId(Guid Id)
        {

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
                    return new BaseResponse<AmenityDto>
                    {
                        Success = true,
                        Message = "Successful",
                        Data = amenity
                    };
                }
                else
                {
                    return new BaseResponse<AmenityDto>
                    {
                        Success = false,
                        Message = "Successful"
                    };
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse<AmenityDto>
                {
                    Success = false,
                    Message = "Successful"
                };
            }



        }


        public async Task<BaseResponse<AmenityDto>> GetAmenityAsync(Guid Id)
        {
            try
            {
                var roomAmenity = await _dbContext.Amenities.FirstOrDefaultAsync(x => x.Id == Id);
                if (roomAmenity != null)
                {
                    return new BaseResponse<AmenityDto>
                    {
                        Message = "",
                        Success = true,
                        Data = new AmenityDto
                        {
                            AmenityName = roomAmenity.AmenityName,
                        }

                    };
                }
                else
                {
                    return new BaseResponse<AmenityDto>
                    {
                        Success = false,
                        Message = "",
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<AmenityDto>
                {
                    Success = false,
                    Message = "",
                };
            }
            
            
        }

        public async Task<BaseResponse<IList<AmenityDto>>> GetAllAmenity()
        {
            try
            {
                var amenity = await _dbContext.Amenities
                           .Select(x => new AmenityDto
                           {
                               AmenityName = x.AmenityName,

                           }).ToListAsync();
                if (amenity != null)
                {
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = true,
                        Message = "Successful"
                    };
                }
                else
                {
                    return new BaseResponse<IList<AmenityDto>>
                    {
                        Success = false,
                        Message = "Successful"
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<IList<AmenityDto>>
                {
                    Success = false,
                    Message = "Successful"
                };
            }
           
           
        }

        public async Task<BaseResponse<AmenityDto>> UpdateAmenity(Guid Id, UpdateAmenity request)
        {

            try
            {
                var amenity = await _dbContext.Amenities.FirstOrDefaultAsync();
                if (amenity == null)
                {
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
                    return new BaseResponse<AmenityDto>
                    {
                        Success = true,
                        Message = "Updated Successful"
                    };
                }
                else
                {
                    return new BaseResponse<AmenityDto>
                    {
                        Success = false,
                        Message = "Update Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<AmenityDto>
                {
                    Success = false,
                    Message = "Update Failed"
                };
            }
           
            
        }
    }
}

