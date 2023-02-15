using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;

namespace Viewer.BookCode
{
    public static class Chapter10_PositionalCamera
    {
        private class CameraEx
        {
            private static readonly double DegreeToRadius = Math.PI / 180;

            private static readonly float ScreenDistance = 1f;
            public Vector3 Origin { get; }

            private Vector3 uDir { get; }
            private Vector3 vDir { get; }
            private Vector3 wDir { get; }

            private float _screenWidth;
            private float _screenHeight;

            private Vector3 _bottomLeftCorner;

            // vfov is top to bottom in degrees
            public CameraEx(Vector3 lookfrom, Vector3 lookat, Vector3 up,  float vfov, float aspect) 
            {
                var theta = vfov * DegreeToRadius;
                var height = Math.Tan(theta/2) * ScreenDistance * 2;
                _screenHeight = (float)height; 
                _screenWidth =  aspect * _screenHeight;

                // 局部坐标系的三个轴u, v , w
                wDir = Vector3.Normalize(lookfrom - lookat);   // 注意这里的方向
                uDir = Vector3.Normalize(Vector3.Cross(up, wDir));
                vDir = Vector3.Cross(wDir, uDir);

                Origin = lookfrom;

                _bottomLeftCorner = Origin - wDir *ScreenDistance - _screenHeight / 2 * vDir - _screenWidth / 2 * uDir;
            }
            public Ray GetRay(float u, float v)
            {
                Vector3 screenPoint = _bottomLeftCorner + (u * _screenWidth * uDir) + (v * _screenHeight * vDir);
                return new Ray(Origin, screenPoint - Origin);  // actually should be "new Ray(origin , curPt - origin)
            }
        }

        public static Image<Rgba32> GenerateImage()
        {
            int nx = 400;
            int ny = 200;

            int ns = 100;  // 抗锯齿采样点数  + 光线混合计算

            Image<Rgba32> image = new(nx, ny);

            CameraEx camera = new CameraEx(
                new Vector3(-2, 2, 1),
                new Vector3(0, 0, -1),
                new Vector3(0, 1, 0),
                90,
                (float)nx / ny);

            IHitable world = new HitableList(
                new[] {
                    new Sphere(new Vector3(0, 0, -1), 0.5f, new Lambertian(new Vector3(0.1f,0.2f,0.5f))),
                    new Sphere(new Vector3(0, -100.5f, -1), 100, new Lambertian(new Vector3(0.8f,0.8f,0))),
                    new Sphere(new Vector3(1,  0, -1), 0.5f, new Metal(new Vector3(0.8f,0.6f,0.2f), 0.3f)),
                    new Sphere(new Vector3(-1, 0, -1), 0.5f, new Dielectric(1.5f)),
                    new Sphere(new Vector3(-1, 0, -1), -0.45f, new Dielectric(1.5f))  // 和前一球叠加起来有这中空球的效果
                });

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

        private static Vector3 GetColor(ref Ray ray, IHitable hitObject, int depth)
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

