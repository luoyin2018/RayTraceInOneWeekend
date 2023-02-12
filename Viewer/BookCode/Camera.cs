using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Viewer.BookCode
{
    public class Camera
    {
        public Vector3 Origin { get; } = Vector3.Zero;

        private float _screenWidth;
        private float _screenHeight;

        private Vector3 _bottomLeftCorner;

        public Camera(float screenWidth, float screenHeight, float screenDistance)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            _bottomLeftCorner = new Vector3(-screenWidth / 2, -screenHeight / 2, -screenDistance);
        }
        public Ray GetRay(float u, float v)
        {
            Vector3 screenPoint = _bottomLeftCorner + new Vector3(u * _screenWidth, v * _screenHeight, 0);
            return  new Ray(Origin, screenPoint);   // actually should be "new Ray(origin , curPt - origin)
        }
    }
}
