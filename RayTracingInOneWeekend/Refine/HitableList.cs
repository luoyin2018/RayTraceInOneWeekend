using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewer.Refine
{
    public class HitableList : IHitable
    {
        private List<IHitable> _hitableObjects;

        public HitableList(IEnumerable<IHitable> hitableObjs)
        {
            _hitableObjects = hitableObjs.ToList();
        }

        public bool Hit(ref Ray ray, float tMin, float tMax, out HitRecord hitRecord)
        {
            bool hit_anything = false;
            hitRecord = new HitRecord();

            float closest_so_far = tMax;
            foreach(var hitableObj in _hitableObjects)
            {
                if(hitableObj.Hit(ref ray, tMin, closest_so_far,out HitRecord hitR))
                {
                    hit_anything = true;
                    closest_so_far = hitR.t;
                    hitRecord = hitR;
                }
            }
            return hit_anything;
        }
    }
}
