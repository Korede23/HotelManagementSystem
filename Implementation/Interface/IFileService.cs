using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Implementation.Interface
{
    public interface IFileService
    {



        string GeneratePhoto(IFormFile photoFile);

        List<string> GeneratePhotos(List<IFormFile> Files);


    }
}
