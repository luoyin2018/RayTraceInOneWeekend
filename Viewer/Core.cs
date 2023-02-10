using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Viewer.BookCode;

namespace Viewer
{
    public static class Core
    {
        public static Image<Rgba32> GenerateImage()
        {
            return Chapter4.GenerateImage();
        }
    }
}
