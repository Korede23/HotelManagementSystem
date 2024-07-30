using HotelManagementSystem.Implementation.Interface;

namespace HotelManagementSystem.Implementation.Services
{
    public class FileService : IFileService
    {

        private readonly IWebHostEnvironment _webhostEnvironment;

        public FileService(IWebHostEnvironment webhostEnvironment)
        {
            _webhostEnvironment = webhostEnvironment;
        }

        public string GeneratePhoto(IFormFile photoFile)
        {
            string path = Path.Combine(_webhostEnvironment.WebRootPath, "RoomImages");
            Directory.CreateDirectory(path);
            string contentType = photoFile.ContentType.Split('/')[1];
            string imagePathToSave = $"HMS-ROOM/{DateTime.Now.ToShortDateString()}/{Guid.NewGuid().ToString().Substring(1, 7)}.{contentType}";
            string fullPath = Path.Combine(path, imagePathToSave);
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                photoFile.CopyTo(fileStream);
            }
            return imagePathToSave;
        }

        public List<string> GeneratePhotos(List<IFormFile> Files)
        {
            var filePaths = new List<string>();
            foreach (var file in Files)
            {
                try
                {
                    // Ensure the "RoomImages" directory exists
                    string path = Path.Combine(_webhostEnvironment.WebRootPath, "RoomImages");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Generate unique file path
                    string contentType = file.ContentType.Split('/')[1];
                    if(contentType.ToLower() == "jpg" ||  contentType.ToLower() == "jpeg" || contentType.ToLower() == "png")
                    {
                        string imagePathToSave = $"HMS-ROOM-{DateTime.Now.ToString("yyyy-MM-dd")}-{Guid.NewGuid().ToString().Substring(1, 7)}.{contentType}";
                        string fullPath = Path.Combine(path, imagePathToSave);

                        // Ensure the subdirectory exists
                        string directoryToSave = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(directoryToSave))
                        {
                            Directory.CreateDirectory(directoryToSave);
                        }

                        // Save the file
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        // Add the saved file path to the list
                        filePaths.Add(imagePathToSave);
                    }
                    
                }
                catch (Exception ex)
                {
                    // Log the error (implement your logging mechanism)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // Optionally, rethrow or handle the exception as needed
                    throw;
                }

            }
            return filePaths;
        }
    }
}
