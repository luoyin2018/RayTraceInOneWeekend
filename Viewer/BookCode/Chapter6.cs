using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;

namespace Viewer.BookCode
{
    public static class Chapter6
    {
        public static Image<Rgba32> GenerateImage()
        {
            int nx = 400;
            int ny = 200;

            int ns = 40;  // 抗锯齿采样点数
            Random rd = new Random();

            Image<Rgba32> image = new(nx, ny);

            Camera camera = new Camera(4, 2, 1);

            IHitable world = new HitableList(
                new[] { 
                    new Sphere(new Vector3(0, 0, -1), 0.5f),
                    new Sphere(new Vector3(0, -100.5f, -1), 100),
                });

            // 从下往上一行一行扫描
            for (int y = ny - 1; y >= 0; y--)
            {
                for (int x = 0; x < nx; x++)
                {
                    Vector3 pxColor = Vector3.Zero;   // 采样混合来抗锯齿
                    for(int s = 0; s<ns; s++)
                    {
                        float v = (float)(ny - 1 - y + rd.NextDouble()) / ny;
                        float u = (float)(x + rd.NextDouble()) / nx;

                        Ray ray = camera.GetRay(u, v);
                        pxColor += GetColor(ref ray, world);
                    }
                    pxColor = pxColor / ns;

                    image[x, y] = new Rgba32(pxColor);
                }
            }

            return image;
        }

        private static Vector3 GetColor(ref Ray ray, IHitable hitObject)
        {
            if (hitObject.Hit(ref ray, 0, float.MaxValue, out HitRecord hitRecord))
            {
                return 0.5f * (hitRecord.normal + Vector3.One);
            }
            var dir = ray.Direction;
            float ratio = 0.5f * (dir.Y + 1.0f);
            return (1.0f - ratio) * Vector3.One + ratio * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}

