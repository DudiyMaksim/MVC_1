namespace MVC_1.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string path);
        void DeleteImage(string path);
    }
}
