using System;
using System.Numerics;

namespace Viewer.BookCode
{
    public interface IMaterial
    {
        bool Scatter(ref Ray ray, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered);
    }

    public class Lambertian : IMaterial
    {
        private Vector3 albedo;

        public Lambertian(Vector3 a)
        {
            albedo = a;
        }

        public bool Scatter(ref Ray ray, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 target = hitRecord.normal + Helper.RandomInUnitSphere();
            scattered = new Ray(hitRecord.hitpoint, target);
            attenuation = albedo;
            return true;
        }
    }

    public class Metal : IMaterial
    {
        private Vector3 albedo;
        private float fuzz;

        public Metal(Vector3 a, float f)
        {
            albedo = a;
            fuzz = f;
        }

        public bool Scatter(ref Ray ray, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 vOut = Helper.Reflect(ray.Direction, hitRecord.normal);
            scattered = new Ray(hitRecord.hitpoint, vOut + fuzz * Helper.RandomInUnitSphere());
            attenuation = albedo;
            return Vector3.Dot(scattered.Direction, hitRecord.normal) > 0;
        }

    }

    public class Helper
    {
        private static Random _rd = new Random();
        public static Vector3 RandomInUnitSphere()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3((float)_rd.NextDouble(), (float)_rd.NextDouble(), (float)_rd.NextDouble())
                    - Vector3.One;
            } while (p.LengthSquared() >= 1);
            return p;
        }
        public static Vector3 RandomInUnitDisk()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3((float)_rd.NextDouble(), (float)_rd.NextDouble(), 0)
                    - new Vector3(1, 1, 0);
            } while (p.LengthSquared() >= 1);
            return p;
        }


        public static double Next()
        {
            return _rd.NextDouble();
        }
        public static float NextFloat()
        {
            return (float)_rd.NextDouble();
        }


        public static Vector3 Reflect(Vector3 vIn, Vector3 normal)
        {
            return vIn - 2 * Vector3.Dot(vIn, normal) * normal;
        }
    }
}
