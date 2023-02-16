using System.Numerics;

namespace RayTracingInOneWeekend.BookCode.Share
{
    public class Material_Metal : IMaterial
    {
        private Vector3 albedo;
        private float fuzz;

        public Material_Metal(Vector3 a, float f)
        {
            albedo = a;
            fuzz = f;
        }

        public bool Scatter(ref Ray ray, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 vOut = MathHelper.Reflect(ray.Direction, hitRecord.normal);
            scattered = new Ray(hitRecord.hitpoint, vOut + fuzz * Randomizer.RandomInUnitSphere());
            attenuation = albedo;
            return Vector3.Dot(scattered.Direction, hitRecord.normal) > 0;
        }
    }
}
