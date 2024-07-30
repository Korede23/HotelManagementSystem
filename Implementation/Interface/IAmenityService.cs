using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HMS.Implementation.Interface
{
    public interface IAmenityService
    {
        Task<BaseResponse<IList<AmenityDto>>> CreateAmenity(CreateAmenityRequestModel request);
        Task<BaseResponse<Guid>> DeleteAmenity(Guid Id);
        Task<BaseResponse<AmenityDto>> GetAmenityBYId(Guid Id);
        Task<BaseResponse<IList<AmenityDto>>> GetAllAmenity();
        Task<BaseResponse<AmenityDto>> UpdateAmenity(Guid Id, UpdateAmenity request);
        Task<BaseResponse<AmenityDto>> GetAmenityAsync(Guid Id);
        Task<List<AmenityDto>> GetAmenity();
    }
}
