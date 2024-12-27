using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTraycing
{
    public class Ray
    {
        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; } 

        public Ray(Vector3 origin, Vector3 direction, bool flag)
        {
            Origin = origin;
            Direction = Vector3.Normalize(direction);
        }

        public Ray(Vector3 origin, Vector3 target)
        {
            Origin = origin;
            Direction = Vector3.Normalize(target - origin);  // Вычисляем направление как разницу между точками и нормализуем
        }

        public Vector3 GetPointAtParameter(float t)
        {
            return Origin + t * Direction;
        }
    }
}
