using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTraycing
{
    public class Rectangle : SceneObject
    {
        public Vector3 P1 { get; set; }  // Одна из вершин прямоугольника
        public Vector3 P2 { get; set; }  // Соседняя вершина (формирует сторону ширины)
        public Vector3 P3 { get; set; }  // Соседняя вершина (формирует сторону высоты)

        public Vector3 Normal { get; set; }

        public Rectangle(Vector3 p1, Vector3 p2, Vector3 p3, Color color, MaterialType materialType)
            : base(color, materialType)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            // Вычисляем нормаль к плоскости прямоугольника
            Vector3 edge1 = P2 - P1;
            Vector3 edge2 = P3 - P1;
            Normal = Vector3.Cross(edge1, edge2);
            Normal = Vector3.Normalize(Normal);
        }


        public override bool Intersect(Ray ray, out float t)
        {
            t = 0;

            // Сначала проверим, пересекает ли луч плоскость прямоугольника
            float denominator = Vector3.Dot(Normal, ray.Direction);
            if (Math.Abs(denominator) < 1e-6)
            {
                return false;
            }

            // Вычисляем точку пересечения луча с плоскостью
            Vector3 rayToP1 = P1 - ray.Origin;
            t = Vector3.Dot(rayToP1, Normal) / denominator;

            if (t < 0)  // Точка пересечения за пределами луча (позади)
            {
                return false;
            }

            // Проверим, лежит ли точка пересечения внутри прямоугольника
            Vector3 intersectionPoint = ray.GetPointAtParameter(t);

            // Векторы от P1 до точки пересечения
            Vector3 v0 = P2 - P1;
            Vector3 v1 = P3 - P1;
            Vector3 v2 = intersectionPoint - P1;

            // Вычисляем скалярные произведения для проверки, находится ли точка внутри прямоугольника (с использованием барицентрических координат)
            float dot00 = Vector3.Dot(v0, v0);
            float dot01 = Vector3.Dot(v0, v1);
            float dot02 = Vector3.Dot(v0, v2);
            float dot11 = Vector3.Dot(v1, v1);
            float dot12 = Vector3.Dot(v1, v2);

            // Вычисляем барицентрические координаты
            float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Проверка для первого треугольника
            bool isInTriangle1 = (u >= 0) && (v >= 0) && (u + v <= 1);

            // Теперь проверим второй треугольник (P1, P3, P4)
            Vector3 P4 = new Vector3((P2.X + P3.X)  - P1.X, (P2.Y + P3.Y)  - P1.Y, (P2.Z + P3.Z)  - P1.Z);
            //Vector3 P4 = P1 + (P3 - P1) - (P2 - P1);

            // Векторы для второго треугольника
            Vector3 v3 = P2 - P4;
            Vector3 v4 = P3 - P4;
            Vector3 v5 = intersectionPoint - P4;

            // Вычисляем скалярные произведения для второго треугольника
            float dot20 = Vector3.Dot(v3, v3);
            float dot21 = Vector3.Dot(v3, v4);
            float dot22 = Vector3.Dot(v3, v5);
            float dot31 = Vector3.Dot(v4, v4);
            float dot32 = Vector3.Dot(v4, v5);

            // Вычисляем барицентрические координаты для второго треугольника
            float invDenom2 = 1.0f / (dot20 * dot31 - dot21 * dot21);
            float u01 = (dot31 * dot22 - dot21 * dot32) * invDenom2;
            float v01 = (dot20 * dot32 - dot21 * dot22) * invDenom2;

            // Проверка для второго треугольника
            bool isInTriangle2 = (u01 >= 0) && (v01 >= 0) && (u01 + v01 <= 1);

            // Если точка лежит внутри хотя бы одного треугольника, то она внутри прямоугольника
            return isInTriangle1 || isInTriangle2;
        }

        public override Vector3 GetIntersectionPoint(Ray ray, float t)
        {
            return ray.GetPointAtParameter(t);
        }

        public override Vector3 PointNormal(Vector3 vector)
        {
            return Normal;
        }
    }
}
