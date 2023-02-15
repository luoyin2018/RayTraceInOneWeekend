using System;
using System.Numerics;

namespace Viewer.BookCode
{
    public class Dielectric : IMaterial
    {
        private readonly float refractIndex;
        public Dielectric(float ri)
        {
            refractIndex = ri;
        }

        public bool Scatter(ref Ray ray, HitRecord hitRecord, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 outwardNormal;
            Vector3 reflected = Helper.Reflect(ray.Direction, hitRecord.normal);
            float ni_over_nt;
            attenuation = Vector3.One;

            float cosine;
            float reflect_prob;

            // ray 是从相机发出的光线,  实际是外界进入相机的光线产生了看到的颜色,这里是反向推.

            if(Vector3.Dot(ray.Direction, hitRecord.normal)>0)   //  物体内部向外部的光线
            {
                outwardNormal = -hitRecord.normal;
                ni_over_nt = refractIndex;

                //cosine = refractIndex * Vector3.Dot(ray.Direction, hitRecord.normal);
                cosine = Vector3.Dot(ray.Direction, hitRecord.normal);
            }
            else   // 物体外部射向内部的光线
            {
                outwardNormal = hitRecord.normal;     
                ni_over_nt = 1 / refractIndex;

                cosine = -Vector3.Dot(ray.Direction, hitRecord.normal);
            }

            if(Refract(ray.Direction, outwardNormal, ni_over_nt, out Vector3 refracted))
            {
                //scattered = new Ray(hitRecord.hitpoint, refracted);
                //return true;
                reflect_prob = Schlick(cosine, refractIndex);
            }
            else
            {
                //scattered = new Ray(hitRecord.hitpoint, reflected);
                //return false;
                reflect_prob = 1;
            }

            if(Helper.Next() < reflect_prob)
            {
                scattered = new Ray(hitRecord.hitpoint, reflected);
            }
            else
            {
                scattered = new Ray(hitRecord.hitpoint, refracted);
            }
            return true;
        }

        float Schlick(float cosin, float refIdx)
        {
            float r0 = (1 - refIdx) / (1 + refIdx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * (float)Math.Pow(1 - cosin, 5);
        }

        bool Refract(Vector3 vIn, Vector3 normal, float ni_over_nt, out Vector3 refacted)
        {
            float dt = Vector3.Dot(vIn, normal);
            float discriminat = 1 - ni_over_nt * ni_over_nt * (1 - dt * dt);
            if(discriminat >0)
            {
                refacted = ni_over_nt * (vIn - normal * dt) - normal * (float)Math.Sqrt(discriminat); 
                return true;
            }
            else
            {
                refacted = Vector3.Zero;
                return false;
            }

        }
    }
}
