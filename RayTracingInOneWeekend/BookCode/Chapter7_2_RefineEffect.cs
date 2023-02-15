using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;

namespace Viewer.BookCode
{
    public static class Chapter7_2_RefineEffect
    {
        private static Random _rd = new Random();
        public static Image<Rgba32> GenerateImage()
        {
            int nx = 200;
            int ny = 100;

            int ns = 100;  // 抗锯齿采样点数  + 漫反射光线计算

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
                        float v = (float)(ny - 1 - y + _rd.NextDouble()) / ny;
                        float u = (float)(x + _rd.NextDouble()) / nx;

                        Ray ray = camera.GetRay(u, v);
                        pxColor += GetColor(ref ray, world);
                    }
                    pxColor = pxColor / ns;

                    pxColor = new Vector3(
                        (float)Math.Sqrt(pxColor.X),
                        (float)Math.Sqrt(pxColor.Y),
                        (float)Math.Sqrt(pxColor.Z)
                        );

                    image[x, y] = new Rgba32(pxColor);
                }
            }

            return image;
        }

        private static Vector3 RandomInUnitSphere()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3((float)_rd.NextDouble(), (float)_rd.NextDouble(), (float)_rd.NextDouble())
                    - Vector3.One;
            } while (p.LengthSquared() >= 1);
            return p;
        }

        private static Vector3 GetColor(ref Ray ray, IHitable hitObject)
        {
            if (hitObject.Hit(ref ray, 0, float.MaxValue, out HitRecord hitRecord))
            {
                Vector3 diffuseDir = hitRecord.normal + RandomInUnitSphere();
                var diffuseRay = new Ray(hitRecord.hitpoint, diffuseDir);
                return 0.5f * GetColor(ref diffuseRay, hitObject);    // 递归 每次光线的反射吸收50%
            }
            var dir = ray.Direction;
            float ratio = 0.5f * (dir.Y + 1.0f);
            return (1.0f - ratio) * Vector3.One + ratio * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}

