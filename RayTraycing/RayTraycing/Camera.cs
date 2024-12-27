using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTraycing
{
    public class Camera
    {
        public Vector3[] Screen { get; }
        public Vector3 Coordinates { get; }

        public Camera(Vector3[] screen, Vector3 coordinates)
        {
            if (screen == null || screen.Length != 4)
            {
                throw new ArgumentException("Screen must be an array of 4 Vector3 elements.");
            }

            Screen = screen;
            Coordinates = coordinates;
        }

        public Vector3 LeftDown()
        {
            return Screen[0];
        }

        public Vector3 LeftTop()
        {
            return Screen[1];
        }

        public Vector3 RightTop()
        {
            return Screen[2];
        }

        public Vector3 RightDown()
        {
            return Screen[3];
        }
    }
}
