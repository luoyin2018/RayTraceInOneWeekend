using System.Numerics;

namespace RayTracingInOneWeekend.BookCode.Share
{
    public struct HitRecord
    {
        public float t;
        public Vector3 hitpoint;
        public Vector3 normal;

        public IMaterial material;
    }
    public interface IHitable
    {
        bool Hit(ref Ray ray, float tMin, float tMax, out HitRecord hitRecord);
    }
}
