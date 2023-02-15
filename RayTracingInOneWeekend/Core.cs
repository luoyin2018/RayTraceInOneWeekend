using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Viewer.BookCode;
using Viewer.Refine;

namespace Viewer
{
    public static class Core
    {
        public static Image<Rgba32> GenerateImage()
        {
            return RandomScene_mrt.GenerateImage();
        }
    }
}
