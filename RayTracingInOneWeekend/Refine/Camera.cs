using System;
using System.Numerics;

namespace RayTracingInOneWeekend.Refine
{
    public class Camera
    {
        private static readonly double DegreeToRadius = Math.PI / 180;

        public Vector3 Origin { get; }

        private readonly Vector3 uDir;
        private readonly Vector3 vDir;
        private readonly Vector3 wDir;

        private readonly float _screenWidth;
        private readonly float _screenHeight;

        private readonly Vector3 _bottomLeftCorner;

        private readonly float lensRadius;

        private readonly Randomizer _rd;
        public Camera(Vector3 lookfrom, Vector3 lookat, Vector3 up, float vfov, float aspect, float aperture, float focusDist, Randomizer rd)
        {
            lensRadius = aperture / 2;

            var theta = vfov * DegreeToRadius;
            var height = Math.Tan(theta / 2) * focusDist * 2;
            _screenHeight = (float)height;
            _screenWidth = aspect * _screenHeight;

            // 局部坐标系的三个轴u, v , w
            wDir = Vector3.Normalize(lookfrom - lookat);   // 注意这里的方向
            uDir = Vector3.Normalize(Vector3.Cross(up, wDir));
            vDir = Vector3.Cross(wDir, uDir);

            Origin = lookfrom;

            _bottomLeftCorner = Origin - wDir * focusDist - _screenHeight / 2 * vDir - _screenWidth / 2 * uDir;

            _rd = rd;
        }
        public Ray GetRay(float u, float v)
        {
            Vector3 rd = lensRadius * _rd.RandomInUnitDisk();
            Vector3 offset = uDir * rd.X + vDir * rd.Y;

            Vector3 screenPoint = _bottomLeftCorner + (u * _screenWidth * uDir) + (v * _screenHeight * vDir);
            return new Ray(Origin + offset, screenPoint - Origin - offset);
        }
    }
}
