using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace View
{
    public static class Core
    {
        public static readonly string picname = @"output.bmp";

        public static void Run()
        {
            int nx = 200;
            int ny = 100;

            using (Image<Rgba32> image = new(nx, ny))
            {
                for (int y = 0; y < ny; y++)
                {
                    for (int x = 0; x < nx; x++)
                    {
                        byte r = (byte)(255.99 * (float)x / (float)nx);

                        byte g = (byte)(255.99 * (float)(ny - y - 1) / (float)ny);

                        image[x, y] = new Rgba32(r, g, 50);
                    }
                }

                image.SaveAsBmp(picname);
            }
        }
    }
}
