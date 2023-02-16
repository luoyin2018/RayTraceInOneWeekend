using System;
using System.Numerics;

namespace RayTracingInOneWeekend.BookCode.Share
{
    public class Camera_Positional
    {
        private static readonly double DegreeToRadius = Math.PI / 180;

        private static readonly float ScreenDistance = 1f;
        public Vector3 Origin { get; }

        private Vector3 uDir { get; }
        private Vector3 vDir { get; }
        private Vector3 wDir { get; }

        private float _screenWidth;
        private float _screenHeight;

        private Vector3 _bottomLeftCorner;

        // vfov is top to bottom in degrees
        public Camera_Positional(Vector3 lookfrom, Vector3 lookat, Vector3 up, float vfov, float aspect)
        {
            var theta = vfov * DegreeToRadius;
            var height = Math.Tan(theta / 2) * ScreenDistance * 2;
            _screenHeight = (float)height;
            _screenWidth = aspect * _screenHeight;

            // 局部坐标系的三个轴u, v , w
            wDir = Vector3.Normalize(lookfrom - lookat);   // 注意这里的方向
            uDir = Vector3.Normalize(Vector3.Cross(up, wDir));
            vDir = Vector3.Cross(wDir, uDir);

            Origin = lookfrom;

            _bottomLeftCorner = Origin - wDir * ScreenDistance - _screenHeight / 2 * vDir - _screenWidth / 2 * uDir;
        }
        public Ray GetRay(float u, float v)
        {
            Vector3 screenPoint = _bottomLeftCorner + (u * _screenWidth * uDir) + (v * _screenHeight * vDir);
            return new Ray(Origin, screenPoint - Origin);  
        }
    }
}
