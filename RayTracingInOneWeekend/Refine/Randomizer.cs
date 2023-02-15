using System;
using System.Numerics;

namespace Viewer.Refine
{
    public class Randomizer
    {
        private Random _rd = new Random();
        public Vector3 RandomInUnitSphere()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3((float)_rd.NextDouble(), (float)_rd.NextDouble(), (float)_rd.NextDouble())
                    - Vector3.One;
            } while (p.LengthSquared() >= 1);
            return p;
        }
        public Vector3 RandomInUnitDisk()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3((float)_rd.NextDouble(), (float)_rd.NextDouble(), 0)
                    - new Vector3(1, 1, 0);
            } while (p.LengthSquared() >= 1);
            return p;
        }


        public double Next()
        {
            return _rd.NextDouble();
        }
        public float NextFloat()
        {
            return (float)_rd.NextDouble();
        }
    }
}
