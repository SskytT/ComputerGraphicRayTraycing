using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTraycing
{
    
    public class Sphere : SceneObject
    {
        public Vector3 Center { get; }
        public float Radius { get; }

        public Sphere(Vector3 center, float radius, Color color, MaterialType materialType)
            : base(color, materialType)
        {
            Center = center;
            Radius = radius;
        }

        public override bool Intersect(Ray ray, out float t)
        {
            t = float.MaxValue;

            Vector3 oc = ray.Origin - Center;

            float a = Vector3.Dot(ray.Direction, ray.Direction);
            float b = 2.0f * Vector3.Dot(oc, ray.Direction);
            float c = Vector3.Dot(oc, oc) - Radius * Radius;

            float discriminant = b * b - 4 * a * c;

            if (discriminant > 0)
            {
                float sqrtDiscriminant = (float)Math.Sqrt(discriminant);

                float t0 = (-b - sqrtDiscriminant) / (2.0f * a);
                float t1 = (-b + sqrtDiscriminant) / (2.0f * a);

                if (t0 > 0)
                {
                    t = t0;
                    return true;
                }
                else if (t1 > 0)
                {
                    t = t1;
                    return true;
                }
            }
            else if (discriminant == 0)
            {
                t = -b / (2.0f * a);
                return t > 0;
            }

            return false;
        }
        public override Vector3 GetIntersectionPoint(Ray ray, float t)
        {
            return ray.Origin + t * ray.Direction;
        }

        public override Vector3 PointNormal(Vector3 vector)
        {
            // Нормаль в точке на сфере = нормализованный вектор от центра сферы к точке
            return Vector3.Normalize(vector - Center);
        }
    }
}
