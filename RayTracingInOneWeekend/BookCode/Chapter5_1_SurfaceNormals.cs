using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Numerics;

namespace Viewer.BookCode
{
    public static class Chapter5_1_SurfaceNormals
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

        private static float HitSphere(Vector3 center, float radius, ref Ray ray)
        {
            Vector3 oc = ray.Origin - center;

            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(oc, ray.Direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;

            float discriminant = b * b - 4 * a * c;

            if(discriminant<0)
            {
                return -1.0f;
            }
            else
            {
                return (-b - (float)Math.Sqrt(discriminant)) / (2f * a);
            }
        }

        private static Vector3 GetColor(ref Ray ray)
        {
            Vector3 sphereCenter = new Vector3(0, 0, -1);
            float radius = 0.5f;
            float t = HitSphere(sphereCenter, radius, ref ray);
            if (t > 0)
            {
                Vector3 hitPoint = ray.PointOnRay(t);
                Vector3 normal = Vector3.Normalize(hitPoint - sphereCenter);
                return 0.5f * (normal + Vector3.One);
            }
            var dir = ray.Direction;
            float ratio = 0.5f * (dir.Y + 1.0f);
            return (1.0f - ratio) * Vector3.One + ratio * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}

