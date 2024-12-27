using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTraycing
{
    public class LightSource
    {
        public Vector3 Position { get; }
        public float Intensity { get; }

        // Конструктор для источника света с заданной позицией и интенсивностью
        public LightSource(Vector3 position, float intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}
