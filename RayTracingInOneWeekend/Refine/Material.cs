using System.Numerics;
namespace RayTracingInOneWeekend.Refine
{
    public interface IMaterial
    {
        bool Scatter(ref Ray ray, HitRecord hitRecord, Randomizer rd,  out Vector3 attenuation, out Ray scattered);
    }
}
