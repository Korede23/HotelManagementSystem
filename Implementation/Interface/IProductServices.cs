using HotelManagementSystem.Dto;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IProductServices
    {
        Task<BaseResponse<Guid>> CreateProduct(CreateProduct request);
        Task<BaseResponse<Guid>> DeleteProductAsync(Guid Id);
        
        Task<BaseResponse<IList<ProductDto>>> GetAllProductsByIdAsync(Guid Id);
        Task<BaseResponse<ProductDto>> GetProductAsync(Guid Id);
        Task<BaseResponse<IList<ProductDto>>> GetAllProductAsync();
        Task<BaseResponse<ProductDto>> UpdateProduct(Guid Id, UpdateProduct request);
    }
}
