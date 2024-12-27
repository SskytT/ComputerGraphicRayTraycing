using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;

namespace RayTraycing
{
    public class Cube: SceneObject
    {
        public List<Rectangle> rectangles;
        public Cube(Vector3 center, float size, Color color, MaterialType materialType)
            : base(color, materialType)
        {
            rectangles = new List<Rectangle>();

            float halfSize = size / 2;

            // Координаты границ куба
            Vector3 min = center - new Vector3(halfSize, halfSize, halfSize);
            Vector3 max = center + new Vector3(halfSize, halfSize, halfSize);

            // Верхняя грань (Y = max.Y)
            rectangles.Add(new Rectangle(
                new Vector3(min.X, max.Y, min.Z),
                new Vector3(max.X, max.Y, min.Z),
                new Vector3(min.X, max.Y, max.Z),
                color, materialType));

            
            // Нижняя грань (Y = min.Y)
            rectangles.Add(new Rectangle(
                new Vector3(min.X, min.Y, min.Z),
                new Vector3(max.X, min.Y, min.Z),
                new Vector3(min.X, min.Y, max.Z),
                color, materialType));

            // Передняя грань (Z = max.Z)
            rectangles.Add(new Rectangle(
                new Vector3(min.X, min.Y, max.Z),
                new Vector3(max.X, min.Y, max.Z),
                new Vector3(min.X, max.Y, max.Z),
                color, materialType));

            // Задняя грань (Z = min.Z)
            rectangles.Add(new Rectangle(
                new Vector3(min.X, min.Y, min.Z),
                new Vector3(max.X, min.Y, min.Z),
                new Vector3(min.X, max.Y, min.Z),
                color, materialType));

            // Левая грань (X = min.X)
            rectangles.Add(new Rectangle(
                new Vector3(min.X, min.Y, min.Z),
                new Vector3(min.X, min.Y, max.Z),
                new Vector3(min.X, max.Y, min.Z),
                color, materialType));

            // Правая грань (X = max.X)
            rectangles.Add(new Rectangle(
                new Vector3(max.X, min.Y, min.Z),
                new Vector3(max.X, min.Y, max.Z),
                new Vector3(max.X, max.Y, min.Z),
                color, materialType));
            
        }

        public override bool Intersect(Ray ray, out float t)
        {
            t = float.MaxValue;
            bool hasIntersection = false;

            foreach (var rectangle in rectangles)
            {
                if (rectangle.Intersect(ray, out float tTemp) && tTemp < t)
                {
                    t = tTemp;
                    hasIntersection = true;
                }
            }

            return hasIntersection;
        }

        public override Vector3 GetIntersectionPoint(Ray ray, float t)
        {
            return ray.GetPointAtParameter(t);
        }

        public override Vector3 PointNormal(Vector3 point)
        {
            foreach (var rectangle in rectangles)
            {
                // Используем метод PointNormal у прямоугольника
                if (Vector3.Dot(rectangle.Normal, point - rectangle.P1) < 1e-6)
                {
                    return rectangle.PointNormal(point);
                }
            }
            return Vector3.Zero; // Случай, когда точка не принадлежит кубу
        }
    }
}
