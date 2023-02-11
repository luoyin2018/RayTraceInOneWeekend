using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Viewer.BookCode
{
    public struct Ray
    {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 o, Vector3 dir)
        {
            Origin = o;
            Direction = Vector3.Normalize(dir);
        }

        public Vector3 PointOnRay(float t)
        {
            return Origin + Direction * t;
        }
    }
}
