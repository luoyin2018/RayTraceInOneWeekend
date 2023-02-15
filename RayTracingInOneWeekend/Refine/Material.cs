using System;
using System.Numerics;
namespace Viewer.Refine
{
    public interface IMaterial
    {
        bool Scatter(ref Ray ray, HitRecord hitRecord, Randomizer rd,  out Vector3 attenuation, out Ray scattered);
    }

    public class Lambertian : IMaterial
    {
        private readonly Vector3 albedo;

        public Lambertian(Vector3 a)
        {
            albedo = a;
        }

        public bool Scatter(ref Ray ray, HitRecord hitRecord, Randomizer rd, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 target = hitRecord.normal + rd.RandomInUnitSphere();
            scattered = new Ray(hitRecord.hitpoint, target);
            attenuation = albedo;
            return true;
        }
    }

    public class Metal : IMaterial
    {
        private readonly Vector3 albedo;
        private readonly float fuzz;

        public Metal(Vector3 a, float f)
        {
            albedo = a;
            fuzz = f;
        }

        public bool Scatter(ref Ray ray, HitRecord hitRecord, Randomizer rd, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 vOut = Helper.Reflect(ray.Direction, hitRecord.normal);
            scattered = new Ray(hitRecord.hitpoint, vOut + fuzz * rd.RandomInUnitSphere());
            attenuation = albedo;
            return Vector3.Dot(scattered.Direction, hitRecord.normal) > 0;
        }
    }

    public class Helper
    {
        public static Vector3 Reflect(Vector3 vIn, Vector3 normal)
        {
            return vIn - 2 * Vector3.Dot(vIn, normal) * normal;
        }
    }
}
