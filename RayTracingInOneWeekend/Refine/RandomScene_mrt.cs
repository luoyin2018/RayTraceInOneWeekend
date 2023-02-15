using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

namespace Viewer.Refine
{
    public static class RandomScene_mrt
    {
        public static Image<Rgba32> GenerateImage()
        {
            int nx = 1200;
            int ny = 900;

            int ns = 200;  

            Image<Rgba32> image = new(nx, ny);

            Vector3 lookfrom = new Vector3(7, 1, 2);
            Vector3 lookat = new Vector3(0, 0, -1);
            float dist_to_focus = (lookfrom - lookat).Length();
            float aperture = 0f;
            float fov = 60;



            IHitable world = GenerateRandomScene();

            var pixels = from y in Enumerable.Range(0, ny)
                        from x in Enumerable.Range(0, nx)
                        select (x, y);

            //var pic =  pixels.AsParallel().Select(pix =>
            pixels.AsParallel().ForAll(pix =>
            {
                Randomizer rd = new Randomizer();
                var camera = new Camera(
                  lookfrom, lookat,
                  new Vector3(0, 1, 0),
                  fov,
                  (float)nx / ny,
                  aperture,
                  dist_to_focus,
                  rd);

                var (x, y) = pix;
                Vector3 pxColor = Vector3.Zero;   // 采样混合来抗锯齿
                for (int s = 0; s < ns; s++)
                {
                    float v = (float)(ny - 1 - y + rd.Next()) / ny;
                    float u = (float)(x + rd.Next()) / nx;

                    Ray ray = camera.GetRay(u, v);
                    pxColor += GetColor(ref ray, world, rd,  0);
                }
                pxColor = pxColor / ns;

                pxColor = new Vector3(
                    (float)Math.Sqrt(pxColor.X),
                    (float)Math.Sqrt(pxColor.Y),
                    (float)Math.Sqrt(pxColor.Z)
                    );
                //return (x, y, pxColor);
                image[x, y] = new Rgba32(pxColor);
            });

            //foreach(var (x,y,color) in pic)
            //{
            //    image[x, y] = new Rgba32(color);
            //}

            return image;
        }

        private static IHitable GenerateRandomScene()
        {
            List<IHitable> hitableObjects = new()
            {
                 new Sphere(new Vector3(0, -1000, 0), 1000, new Lambertian(new Vector3(0.5f,0.5f,0.5f)))
            };

            Randomizer rd = new Randomizer();

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    double chooseMat = rd.Next();
                    Vector3 center = new Vector3(a + 0.9f * rd.NextFloat(), 0.2f, b + 0.9f * rd.NextFloat());

                    if ((center - new Vector3(4, 0.2f, 0)).Length() < 0.9f
                        || (center - new Vector3(-4, 0.2f, 0)).Length() < 0.9f
                        || (center - new Vector3(0, 0.2f, 0)).Length() < 0.9f)
                    {
                        continue;
                    }

                    if (chooseMat < 0.7)
                    {
                        hitableObjects.Add(
                            new Sphere(
                                center,
                                0.2f,
                                new Lambertian(new Vector3(rd.NextFloat() * rd.NextFloat(), rd.NextFloat() * rd.NextFloat(), rd.NextFloat() * rd.NextFloat()))));
                    }
                    else if (chooseMat < 0.9)
                    {
                        hitableObjects.Add(
                            new Sphere(
                                center,
                                0.2f,
                                new Metal(new Vector3(0.5f * (1 + rd.NextFloat()), 0.5f * (1 + rd.NextFloat()), 0.5f * (1 + rd.NextFloat())), 0.5f * rd.NextFloat())));
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

            hitableObjects.Add(new Sphere(new Vector3(0, 1, 0), 1, new Dielectric(1.5f)));
            hitableObjects.Add(new Sphere(new Vector3(-4, 1, 0), 1, new Lambertian(new Vector3(0.4f,0.2f,0.1f))));
            hitableObjects.Add(new Sphere(new Vector3(4, 1, 0), 1, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0)));

            return new HitableList(hitableObjects);
        }

        public static Vector3 GetColor(ref Ray ray, IHitable hitObject,Randomizer rd, int depth)
        {
            if (hitObject.Hit(ref ray, 0.001f, float.MaxValue, out HitRecord hitRecord))
            {
                if (depth < 50 && hitRecord.material.Scatter(ref ray, hitRecord, rd, out Vector3 attenuation, out Ray scattered))
                {
                    return attenuation * GetColor(ref scattered, hitObject,rd, depth + 1);
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

