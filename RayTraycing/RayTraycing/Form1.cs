using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace RayTraycing
{
    public partial class Form1 : Form
    {
        public Camera camera;
        public List<SceneObject> sceneObjects;
        public List<LightSource> lightSources;

        public Form1()
        {
            InitializeComponent();
            InitializeScene();
        }

        // Инициализация сцены
        private void InitializeScene()
        {
            // Камера с углами экрана
            Vector3[] screenCorners = {
                new Vector3(-20, -20, 35), // Верхний левый угол
                new Vector3(-20, 20, 35), // Нижний левый угол
                new Vector3(20, 20, 35), // Нижний правый угол
                new Vector3(20, -20, 35)  // Верхний правый угол
            };
            camera = new Camera(screenCorners, new Vector3(0, 0, -17)); // Камера на позиции (0, 0, -5)

            // Источник света
            lightSources = new List<LightSource>
            {
                //new LightSource(new Vector3(-1.5f, -2.5f, 0), 1f), // Источник света в верхней части сцены
                //new LightSource(new Vector3(1.5f, 2.5f, 0), 1f) // Источник света в нижней части сцены
                new LightSource(new Vector3(-2, 0, 0), 1.5f),
                new LightSource(new Vector3(2, 1, 0), 2f)
            };

            // Сцена с прямоугольниками (стены комнаты)
            sceneObjects = new List<SceneObject>();

            sceneObjects.Add(new Rectangle(new Vector3(-5, -5, 5), new Vector3(-5, 5, 5), new Vector3(5, -5, 5), Color.Green, MaterialType.Diffuse));
            sceneObjects.Add(new Rectangle(new Vector3(5, -5, 5), new Vector3(5, 5, 5), new Vector3(5, -5, -5), Color.Red, MaterialType.Diffuse));
            sceneObjects.Add(new Rectangle(new Vector3(-5, -5, 5), new Vector3(-5, -5, -5), new Vector3(-5, 5, 5), Color.Blue, MaterialType.Diffuse));
            sceneObjects.Add(new Rectangle(new Vector3(-5, -5, -5), new Vector3(-5, -5, 5), new Vector3(5, -5, -5), Color.Gray, MaterialType.Diffuse));
            sceneObjects.Add(new Rectangle(new Vector3(-5, 5, -5), new Vector3(5, 5, -5), new Vector3(-5, 5, 5), Color.Gray, MaterialType.Diffuse));

            Cube cube1 = new Cube(new Vector3(2f, 3f, 2f), 2f, Color.Yellow, MaterialType.Diffuse);
            cube1.rectangles[3].Normal *= -1;
            for (int i = 0; i < cube1.rectangles.Count; i++)
            {
                sceneObjects.Add(cube1.rectangles[i]);
            }

            Cube cube2 = new Cube(new Vector3(-2, 3, 2f), 2f, Color.Pink, MaterialType.Diffuse);
            cube2.rectangles[3].Normal *= -1;
            for (int i = 0; i < cube2.rectangles.Count; i++)
            {
                sceneObjects.Add(cube2.rectangles[i]);
            }

            Sphere sphere1 = new Sphere(new Vector3(2f, 0f, 3f), 1.5f, Color.Red, MaterialType.Diffuse);
            sceneObjects.Add(sphere1);

            Sphere sphere2 = new Sphere(new Vector3(-2f, 0f, -5f), 1f, Color.White, MaterialType.Diffuse);
            sceneObjects.Add(sphere2);

            Render();
        }

        public void Render()
        {
            Bitmap bitmap = new Bitmap(Screen.Width, Screen.Height);
            for (int i = 0; i < Screen.Width; i++)
            {
                for (int j = 0; j < Screen.Height; j++)
                {
                    Ray ray = GetRay(new Point(i, j));
                    float minLength = float.MaxValue;
                    int minLengthIndex = -1;
                    int cnt = 0;
                    Color pixelColor = Color.Black;
                    foreach (SceneObject obj in sceneObjects)
                    {
                        if (obj.Intersect(ray, out float t))
                        {
                            Vector3 intersection = obj.GetIntersectionPoint(ray, t);
                            float length = (intersection - ray.Origin).Length();
                            if (length < minLength)
                            {
                                minLength = length;
                                minLengthIndex = cnt;

                                pixelColor = GetLightingColor(intersection, obj);
                            }
                        }
                        cnt++;
                    }
                    if (minLengthIndex != -1)
                    {
                        bitmap.SetPixel(i, j, pixelColor);
                    }
                    else
                    {
                        bitmap.SetPixel(i, j, Color.Black);
                    }
                }
            }
            Screen.Image = bitmap;
        }

        
        public Ray GetRay(Point pixel)
        {
            float minX = camera.Screen[0].X;
            float maxX = camera.Screen[2].X;
            float minY = camera.Screen[0].Y;
            float maxY = camera.Screen[2].Y;
            
            float screenWidth = maxX - minX;
            float screenHeight = maxY - minY;

            float percentX = (float)pixel.X / (float)Screen.Width;
            float percentY = (float)pixel.Y / (float)Screen.Height;

            float finalX = minX + screenWidth * percentX;
            float finalY = minY + screenHeight * percentY;

            return new Ray(camera.Coordinates, new Vector3(finalX, finalY, camera.Screen[0].Z));
        }
        
        public Color GetLightingColor(Vector3 intersectionPoint, SceneObject obj)
        {
            // Начальный цвет объекта (без учета освещения)
            Color baseColor = obj.Color;
            Vector3 finalColor = new Vector3(0, 0, 0);

            foreach (var light in lightSources)
            {
                // Вектор от точки пересечения до источника света
                Vector3 lightDir = light.Position - intersectionPoint;
                float lightDistance = lightDir.Length();
                lightDir = Vector3.Normalize(lightDir);

                // Проверка на тени
                bool isShadowed = false;
                Ray shadowRay = new Ray(intersectionPoint + lightDir * 1e-3f, lightDir, true); // Немного сместим начало луча
                foreach (var sceneObject in sceneObjects)
                {
                    if (sceneObject.Intersect(shadowRay, out float t) && t < lightDistance)
                    {
                        isShadowed = true;
                        break;
                    }
                }

                // Если точка в тени, пропускаем этот источник света
                if (isShadowed)
                {
                    continue;
                }

                // Нормаль к поверхности объекта
                Vector3 normal = obj.PointNormal(intersectionPoint);

                // Косинус угла между нормалью и вектором к источнику света (скалярное произведение)
                float intensity = Math.Max(Vector3.Dot(normal, lightDir), 0) * light.Intensity / (lightDistance * lightDistance * 0.1f);

                finalColor.X += baseColor.R * intensity;
                finalColor.Y += baseColor.G * intensity;
                finalColor.Z += baseColor.B * intensity;
            }

            // Ограничиваем цветовые значения в диапазоне [0, 255]
            return Color.FromArgb(
                (int)Math.Clamp(finalColor.X, 0, 255),
                (int)Math.Clamp(finalColor.Y, 0, 255),
                (int)Math.Clamp(finalColor.Z, 0, 255)
            );
        }
        
    }
}
