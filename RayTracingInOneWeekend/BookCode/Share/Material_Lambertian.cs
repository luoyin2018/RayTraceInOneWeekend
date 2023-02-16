using System.Numerics;

namespace RayTracingInOneWeekend.BookCode.Share
{
    public class Materila_Lambertian : IMaterial
    {
        private Vector3 albedo;

        public Materila_Lambertian(Vector3 a)
        {
            albedo = a;
        }

        public bool Scatter(ref Ray ray, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 target = hitRecord.normal + Randomizer.RandomInUnitSphere();
            scattered = new Ray(hitRecord.hitpoint, target);
            attenuation = albedo;
            return true;
        }
    }
}
