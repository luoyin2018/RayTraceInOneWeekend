using System;
using System.Numerics;

namespace RayTracingInOneWeekend.Refine
{
    public class Sphere : IHitable
    {
        public Vector3 Center { get; }
        public float Radius { get; }
        public IMaterial Material { get; }

        public Sphere(Vector3 center, float radius, IMaterial mat)
        {
            Center = center;
            Radius = radius;
            Material = mat;
        }

        public bool Hit(ref Ray ray, float tMin, float tMax, out HitRecord hitRecord)
        {
            Vector3 oc = ray.Origin - Center;

            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(oc, ray.Direction);
            float c = Vector3.Dot(oc, oc) - Radius * Radius;

            float discriminant = b * b - 4 * a * c;

            hitRecord = new HitRecord();
            if (discriminant > 0)
            {
                float tHit = tMin - 1;

                float t_temp1 = (-b - (float)Math.Sqrt(discriminant)) / (2f * a);
                if (t_temp1 > tMin && t_temp1 < tMax)
                {
                    tHit = t_temp1;
                }
                else
                {
                    float t_temp2 = (-b + (float)Math.Sqrt(discriminant)) / (2f * a);
                    if (t_temp2 > tMin && t_temp2 < tMax)
                    {
                        tHit = t_temp2;
                    }
                }

                if(tHit>tMin)
                {
                    hitRecord.t = tHit;
                    hitRecord.hitpoint = ray.PointOnRay(tHit);
                    hitRecord.normal = (hitRecord.hitpoint - Center) / Radius;
                    hitRecord.material = Material;

                    return true;
                }
            }

            return false;
        }
    }
}
