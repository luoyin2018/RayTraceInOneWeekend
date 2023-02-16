using System.Numerics;

namespace RayTracingInOneWeekend.BookCode.Share
{
    public interface IMaterial
    {
        bool Scatter(ref Ray ray, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered);
    }
}
