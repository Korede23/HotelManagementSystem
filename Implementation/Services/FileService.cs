using HotelManagementSystem.Implementation.Interface;

namespace HotelManagementSystem.Implementation.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webhostEnvironment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment webhostEnvironment, ILogger<FileService> logger)
        {
            _webhostEnvironment = webhostEnvironment;
            _logger = logger;
        }

        public string GeneratePhoto(IFormFile photoFile)
        {
            _logger.LogInformation("GeneratePhoto method called.");

            try
            {
                string path = Path.Combine(_webhostEnvironment.WebRootPath, "RoomImages");
                Directory.CreateDirectory(path);
                string contentType = photoFile.ContentType.Split('/')[1];
                string imagePathToSave = $"HMS-ROOM/{DateTime.Now:yyyy-MM-dd}/{Guid.NewGuid():N}. {contentType}";
                string fullPath = Path.Combine(path, imagePathToSave);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    photoFile.CopyTo(fileStream);
                }

                _logger.LogInformation("Photo generated successfully and saved at: {FullPath}", fullPath);
                return imagePathToSave;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating photo.");
                throw;
            }
        }

        public List<string> GeneratePhotos(List<IFormFile> files)
        {
            _logger.LogInformation("GeneratePhotos method called with {FileCount} files.", files.Count);

            var filePaths = new List<string>();
            foreach (var file in files)
            {
                try
                {
                    string path = Path.Combine(_webhostEnvironment.WebRootPath, "RoomImages");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string contentType = file.ContentType.Split('/')[1];
                    if (contentType.ToLower() == "jpg" || contentType.ToLower() == "jpeg" || contentType.ToLower() == "png")
                    {
                        string imagePathToSave = $"HMS-ROOM-{DateTime.Now:yyyy-MM-dd}-{Guid.NewGuid():N}.{contentType}";
                        string fullPath = Path.Combine(path, imagePathToSave);

                        string directoryToSave = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(directoryToSave))
                        {
                            Directory.CreateDirectory(directoryToSave);
                        }

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        filePaths.Add(imagePathToSave);
                        _logger.LogInformation("Photo saved at: {FullPath}", fullPath);
                    }
                    else
                    {
                        _logger.LogWarning("Unsupported file type: {ContentType}", contentType);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while generating photo for file: {FileName}", file.FileName);
                    // Optionally, rethrow or handle the exception as needed
                    throw;
                }
            }
            return filePaths;
        }
    }
}
