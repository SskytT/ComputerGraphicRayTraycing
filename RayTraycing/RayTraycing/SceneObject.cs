using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTraycing
{
    public enum MaterialType
    {
        Reflective = 0, // Зеркальный материал
        Diffuse = 1     // Диффузный материал
    }

    public abstract class SceneObject
    {
        public Color Color { get; set; }

        public MaterialType MaterialType { get; set; }

        protected SceneObject(Color color, MaterialType materialType)
        {
            Color = color;
            MaterialType = materialType;
        }

        public abstract bool Intersect(Ray ray, out float t);

        public abstract Vector3 GetIntersectionPoint(Ray ray, float t);

        public abstract Vector3 PointNormal(Vector3 vector);
    }
}
