using System.Numerics;
namespace RayTracingInOneWeekend.Refine
{

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
            Vector3 vOut = MathHelper.Reflect(ray.Direction, hitRecord.normal);
            scattered = new Ray(hitRecord.hitpoint, vOut + fuzz * rd.RandomInUnitSphere());
            attenuation = albedo;
            return Vector3.Dot(scattered.Direction, hitRecord.normal) > 0;
        }
    }
}
