using System;
using System.Numerics;

namespace RayTracingInOneWeekend.BookCode.Share
{
    public static class Randomizer
    {
        private static Random _rd = new Random();
        public static Vector3 RandomInUnitSphere()
        {
            Vector3 p;
            do
            {
                p = 2.0f * RandomVector3() - Vector3.One;
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

        public static Vector3 RandomVector3()
        {
            return new Vector3((float)_rd.NextDouble(), (float)_rd.NextDouble(), (float)_rd.NextDouble());
        }

        public static double Next()
        {
            return _rd.NextDouble();
        }
        public static float NextFloat()
        {
            return (float)_rd.NextDouble();
        }
    }
}
