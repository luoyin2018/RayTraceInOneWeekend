using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;
using System.Collections.Generic;

namespace Viewer.BookCode
{
    public static class Chapter12_RandomScene
    {
        public static Image<Rgba32> GenerateImage()
        {
            int nx = 100;
            int ny = 50;

            int ns = 50;  

            Image<Rgba32> image = new(nx, ny);

            Vector3 lookfrom = new Vector3(6, 2, 2);
            Vector3 lookat = new Vector3(0, 0, -1);
            float dist_to_focus = (lookfrom - lookat).Length();
            float aperture = 0.02f;
            float fov = 90;

            var camera = new Chapter11_DefocusBlur.CameraEx(
                lookfrom,lookat,
                new Vector3(0, 1, 0),
                fov,
                (float)nx / ny,
                aperture,
                dist_to_focus);

            IHitable world = GenerateRandomScene();

            // 从下往上一行一行扫描
            for (int y = ny - 1; y >= 0; y--)
            {
                for (int x = 0; x < nx; x++)
                {
                    Vector3 pxColor = Vector3.Zero;   // 采样混合来抗锯齿
                    for (int s = 0; s < ns; s++)
                    {
                        float v = (float)(ny - 1 - y + Helper.Next()) / ny;
                        float u = (float)(x + Helper.Next()) / nx;

                        Ray ray = camera.GetRay(u, v);
                        pxColor += Chapter11_DefocusBlur.GetColor(ref ray, world, 0);
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

        private static IHitable GenerateRandomScene()
        {
            List<IHitable> hitableObjects = new()
            {
                 new Sphere(new Vector3(0, -1000, 0), 1000, new Lambertian(new Vector3(0.5f,0.5f,0.5f)))
            };

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    double chooseMat = Helper.Next();
                    Vector3 center = new Vector3(a + 0.9f * Helper.NextFloat(), 0.2f, b + 0.9f * Helper.NextFloat());
                    if ((center - new Vector3(4, 0.2f, 0)).Length() > 0.9f)
                    {
                        if (chooseMat < 0.8)
                        {
                            hitableObjects.Add(
                                new Sphere(
                                    center,
                                    0.2f,
                                    new Lambertian(new Vector3(Helper.NextFloat() * Helper.NextFloat(), Helper.NextFloat() * Helper.NextFloat(), Helper.NextFloat() * Helper.NextFloat()))));
                        }
                        else if (chooseMat < 0.95)
                        {
                            hitableObjects.Add(
                                new Sphere(
                                    center,
                                    0.2f,
                                    new Metal(new Vector3(0.5f * (1 + Helper.NextFloat()), 0.5f * (1 + Helper.NextFloat()), 0.5f * (1 + Helper.NextFloat())), 0.5f * Helper.NextFloat())));
                        }
                        else
                        {
                            hitableObjects.Add(
                                new Sphere(
                                    center,
                                    0.2f,
                                    new Dielectric(1.5f)));

                        }
                    } 
                }
            }

            hitableObjects.Add(new Sphere(new Vector3(0, 1, 0), 1, new Dielectric(1.5f)));
            hitableObjects.Add(new Sphere(new Vector3(-4, 1, 0), 1, new Lambertian(new Vector3(0.4f,0.2f,0.1f))));
            hitableObjects.Add(new Sphere(new Vector3(4, 1, 0), 1, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0)));

            return new HitableList(hitableObjects);
        }
    }
}

