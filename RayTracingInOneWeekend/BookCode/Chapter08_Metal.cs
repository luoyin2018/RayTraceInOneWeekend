using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;
using RayTracingInOneWeekend.BookCode.Share;

namespace RayTracingInOneWeekend.BookCode
{
    public class Chapter08_Metal : IImageGenerator
    {
        public Image<Rgba32> GenerateImage()
        {
            int nx = 400;
            int ny = 200;

            int ns = 100;  // 抗锯齿采样点数  + 漫反射光线计算

            Image<Rgba32> image = new(nx, ny);

            Camera_Basic camera = new Camera_Basic(4, 2, 1);

            IHitable world = new HitableList(
                new[] {
                    new Sphere(new Vector3( 0,       0, -1), 0.5f, new Materila_Lambertian(new Vector3(0.8f, 0.3f, 0.3f))),
                    new Sphere(new Vector3( 0, -100.5f, -1),  100, new Materila_Lambertian(new Vector3(0.8f, 0.8f, 0))),
                    new Sphere(new Vector3( 1,       0, -1), 0.5f, new Material_Metal(new Vector3(0.8f, 0.6f, 0.2f), 0.3f)),
                    new Sphere(new Vector3(-1,       0, -1), 0.5f, new Material_Metal(new Vector3(0.8f, 0.8f, 0.8f), 1.0f))
                });

            // 从下往上一行一行扫描
            for (int y = ny - 1; y >= 0; y--)
            {
                for (int x = 0; x < nx; x++)
                {
                    Vector3 pxColor = Vector3.Zero;   // 采样混合来抗锯齿
                    for(int s = 0; s<ns; s++)
                    {
                        float v = (ny - 1 - y + Randomizer.NextFloat()) / ny;
                        float u = (x + Randomizer.NextFloat()) / nx;

                        Ray ray = camera.GetRay(u, v);
                        pxColor += GetColor(ref ray, world, 0);
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

        public static Vector3 GetColor(ref Ray ray, IHitable hitObject, int depth)
        {
            if (hitObject.Hit(ref ray, 0.001f, float.MaxValue, out HitRecord hitRecord))
            {
                if (depth < 50 && hitRecord.material.Scatter(ref ray, hitRecord, out Vector3 attenuation, out Ray scattered))
                {
                    return attenuation * GetColor(ref scattered, hitObject, depth + 1);
                }
                else
                {
                    return Vector3.Zero;
                }
            }
            var dir = ray.Direction;
            float ratio = 0.5f * (dir.Y + 1.0f);
            return (1.0f - ratio) * Vector3.One + ratio * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}

