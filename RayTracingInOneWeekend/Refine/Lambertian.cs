using System.Numerics;
namespace RayTracingInOneWeekend.Refine
{
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
}
