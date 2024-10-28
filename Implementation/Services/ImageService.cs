using HotelManagementSystem.Dto.RequestModel;
using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Model.Entity;
using HotelManagementSystem.Models.Entity;

namespace HotelManagementSystem.Implementation.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly ILogger<ImageService> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly string _imageFolderPath;

        public ImageService(ApplicationDbContext dbContext, IFileService fileService, ILogger<ImageService> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _logger = logger;
            _env = env;
            _imageFolderPath = Path.Combine(env.ContentRootPath, "wwwroot", "ProductImages");
        }

        public async Task<BaseResponse<ImageDto>> AddImageAsync(CreateImage request)
        {
            _logger.LogInformation("AddImageAsync called with RoomId: {RoomId}", request.RoomId);

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

                    _logger.LogInformation("Image created successfully with Id: {ImageId}", image.Id);

                    return new BaseResponse<ImageDto>
                    {
                        Success = true,
                        Message = "Image created successfully.",
                        Data = new ImageDto
                        {
                            Id = image.Id,
                            RoomId = request.RoomId,
                        }
                    };
                }

                _logger.LogWarning("Invalid request received in AddImageAsync.");
                return new BaseResponse<ImageDto>
                {
                    Success = false,
                    Message = "Invalid request."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Image creation failed for RoomId: {RoomId}", request.RoomId);
                return new BaseResponse<ImageDto>
                {
                    Success = false,
                    Message = $"Image creation failed: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<int>> AddImagesAsync(List<IFormFile> requestPhotoFiles, Guid roomId)
        {
            _logger.LogInformation("AddImagesAsync called with RoomId: {RoomId} and {PhotoCount} photos", roomId, requestPhotoFiles.Count);

            var response = new BaseResponse<int>();

            try
            {
                var images = new List<Images>();
                int saveChangesRows = 0;

                if (requestPhotoFiles.Count > 0)
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
                            _logger.LogInformation("{PhotoCount} images created successfully for RoomId: {RoomId}", images.Count, roomId);
                            return new BaseResponse<int>
                            {
                                Success = true,
                                Message = "Image creation successful.",
                                Data = saveChangesRows,
                            };
                        }
                    }
                }

                _logger.LogWarning("No images added for RoomId: {RoomId}", roomId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Image creation failed for RoomId: {RoomId}", roomId);
                return new BaseResponse<int>
                {
                    Success = false,
                    Message = $"Image creation failed: {ex.Message}"
                };
            }

            return response;
        }

        public async Task<List<string>> AddProductImagesAsync(IFormFileCollection files)
        {
            var imagePaths = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    
                    var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    var filePath = Path.Combine(_imageFolderPath, fileName);

                    Directory.CreateDirectory(_imageFolderPath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    imagePaths.Add("ProductImages/" + fileName); 
                }
            }

            return imagePaths;
        }

    }
}
