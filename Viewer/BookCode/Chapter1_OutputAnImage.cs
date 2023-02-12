using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Viewer.BookCode
{
    public static class Chapter1_OutputAnImage
    {
        public static Image<Rgba32> GenerateImage()
        {
            int nx = 200;
            int ny = 100;

            Image<Rgba32> image = new(nx, ny);
            //for (int y = ny - 1; y >= 0; y--)
            for (int y = 0; y < ny; y++)
            {
                for (int x = 0; x < nx; x++)
                {
                    byte r = (byte)(255.99 * x / nx);

                    byte g = (byte)(255.99 * (ny - y) / ny);

                    image[x, y] = new Rgba32(r, g, 50);
                }
            }
            return image;
        }
    }
}
