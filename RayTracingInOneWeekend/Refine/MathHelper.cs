using System.Numerics;
namespace RayTracingInOneWeekend.Refine
{
    public class MathHelper
    {
        public static Vector3 Reflect(Vector3 vIn, Vector3 normal)
        {
            return vIn - 2 * Vector3.Dot(vIn, normal) * normal;
        }
    }
}
