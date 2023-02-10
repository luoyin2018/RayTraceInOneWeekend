using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

namespace Viewer.BookCode
{
    public static class Chapter3
    {
        public static Image<Rgba32> GenerateImage()
        {
            int nx = 400;
            int ny = 200;
            Image<Rgba32> image = new(nx, ny);

            Vector3 origin = Vector3.Zero;

            float width = 4;
            float height = 2;
            float dist = 1;    // 画面距相机的距离

            float widthStep = width / (nx - 1);
            float heightStep = height / (ny - 1);

            Vector3 bottom_left_corner = new Vector3(-width / 2, -height / 2, -dist);

            // 从下往上一行一行扫描
            for (int y = ny - 1; y >= 0; y--)
            {
                int dy = ny - 1 - y;

                for (int x = 0; x < nx; x++)
                {
                    Vector3 curPt = bottom_left_corner + new Vector3(x * widthStep, dy * heightStep, 0);

                    Ray ray = new Ray(origin, curPt);   // actually should be "new Ray(origin , curPt - origin)

                    image[x, y] = new Rgba32(GetColor(ref ray));
                }
            }

            return image;
        }

        private static Vector3 GetColor(ref Ray ray)
        {
            var dir = ray.Direction;
            float t = 0.5f * (dir.Y + 1.0f);
            return (1.0f - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}

