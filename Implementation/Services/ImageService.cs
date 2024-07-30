using Azure.Core;
using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;

        public ImageService(ApplicationDbContext dbContext, IFileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }
        public async Task<BaseResponse<ImageDto>> AddImageAsync(CreateImage request)
        {
            try
            {
                if (request != null)
                {
                    var image = new Images
                    {
                        ImagePath = _fileService.GeneratePhoto(request.ImageFile),
                        RoomId = request.RoomId,
                    };
                    await _dbContext.Images.AddAsync(image);
                    await _dbContext.SaveChangesAsync();

                    return new BaseResponse<ImageDto>
                    {
                        Success = true,
                        Message = $"Image created successfully.",
                        Data = new ImageDto
                        {
                            Id = image.Id,
                            RoomId = request.RoomId,
                        }
                    };
                }

                return new BaseResponse<ImageDto>
                {
                    Success = false,
                    Message = "Invalid request."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ImageDto>
                {
                    Success = false,
                    Message = $"Image creation failed: {ex.Message}"
                };
            }


        }

        public async Task<BaseResponse<int>> AddImagesAsync(List<IFormFile> requestPhotoFiles, Guid roomId)
        {
            var response = new BaseResponse<int>();

            try
            {

                var images = new List<Images>();
                int saveChangesRows = 0;
                if (requestPhotoFiles.Count != 0)
                {
                    var photoFiles = _fileService.GeneratePhotos(requestPhotoFiles);
                    foreach (var file in photoFiles)
                    {
                        var image = new Images
                        {
                            ImagePath = file,
                            RoomId = roomId
                        };
                        images.Add(image);
                    }
                    if (images.Count > 0)
                    {
                        await _dbContext.Images.AddRangeAsync(images);
                        saveChangesRows = await _dbContext.SaveChangesAsync();

                        if (saveChangesRows > 0)
                        {
                            return response = new BaseResponse<int>
                            {
                                Success = true,
                                Message = $"Image creation successful.",
                                Data = saveChangesRows,
                            };
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return response = new BaseResponse<int>
                {
                    Success = false,
                    Message = $"Image creation failed: {ex.Message}"
                };
            }

            return response;


        }

        //public async Task<BaseResponse<Guid>> DeleteImageAsync(Guid Id)
        //{
        //    try
        //    {
        //        var image = await _dbContext.Images.FirstOrDefaultAsync(r => r.Id == Id);
        //        if (image == null)
        //        {
        //            return new BaseResponse<Guid>
        //            {
        //                Success = false,
        //                Message = $"Image  not found."
        //            };
        //        }

        //        _dbContext.Images.Remove(image);
        //        if (await _dbContext.SaveChangesAsync() > 0)
        //        {
        //            return new BaseResponse<Guid>
        //            {
        //                Success = true,
        //                Message = $"Image has been deleted successfully.",
        //                Data = Id
        //            };
        //        }
        //        else
        //        {
        //            return new BaseResponse<Guid>
        //            {
        //                Success = false,
        //                Message = $"Failed to delete image There was an error in the deletion process."
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<Guid>
        //        {
        //            Success = false,
        //            Message = $"An error occurred while deleting the image: {ex.Message}"
        //        };
        //    }
        //}
    }
}
