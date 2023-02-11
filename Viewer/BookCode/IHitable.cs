using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Viewer
{
    public struct HitRecord
    {
        public float t;
        public Vector3 hitpoint;
        public Vector3 normal;
    }
    public interface IHitable
    {
        bool Hit(ref Ray ray, float tMin, float tMax, out HitRecord hitRecord);
    }
}
