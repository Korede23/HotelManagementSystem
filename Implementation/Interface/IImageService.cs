using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using System.Threading.Tasks;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IImageService
    {
        Task<BaseResponse<ImageDto>> AddImageAsync(CreateImage request);
        Task<BaseResponse<int>> AddImagesAsync(List<IFormFile> requestPhotoFiles, Guid roomId);
    }
}
