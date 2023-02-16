using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingInOneWeekend
{
    public interface IImageGenerator
    {
        public Image<Rgba32> GenerateImage();
    }
}
