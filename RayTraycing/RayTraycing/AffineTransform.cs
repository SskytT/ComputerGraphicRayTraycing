using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTraycing
{
    public class AffineTransform
    {
        //функция для афинных преобразований точки в пространстве
        public static void AffineTransformation(Vector3 p, float[,] TransMatrix)
        {
            float[,] matrix_p = new float[1, 4] { { p.X, p.Y, p.Z, 1 } };
            float[,] matrix_res = Matrix.Multiplication(matrix_p, TransMatrix);
            p.X = matrix_res[0, 0];
            p.Y = matrix_res[0, 1];
            p.Z = matrix_res[0, 2];
        }

        //функция для  афинных преобразования списка точек в пространстве
        public static void AffineTransformationList(List<Vector3> l, float[,] TransMatrix)
        {
            for (int i = 0; i < l.Count; i++)
            {
                AffineTransformation(l[i], TransMatrix);
            }
        }

        //Сдвиг списка точек на dx, dy, dz
        public static void Offset(List<Vector3> l, float dx, float dy, float dz)
        {
            float[,] transMatrix = new float[4, 4] {
                                                { 1, 0, 0, 0 },
                                                { 0, 1, 0, 0 },
                                                { 0, 0, 1, 0 },
                                                { dx, dy, dz, 1 }
                                              };
            AffineTransformationList(l, transMatrix);
        }

        //Масштабирование относительно центра в пространстве на коэффицента a, b, c
        public static void ScallingRelativeCenter(List<Vector3> l, float a, float b, float c)
        {
            float[,] transMatrix = new float[4, 4] {
                                                { a, 0, 0, 0 },
                                                { 0, b, 0, 0 },
                                                { 0, 0, c, 0 },
                                                { 0, 0, 0, 1 }
                                              };
            AffineTransformationList(l, transMatrix);
        }

        //Масштабирование относительно точки в пространстве
        public static void ScallingRelativePoint(List<Vector3> l, float a, float b, float c, float x, float y, float z)
        {
            Offset(l, -x, -y, -z);
            ScallingRelativeCenter(l, a, b, c);
            Offset(l, x, y, z);
        }

        //отражение относитльено плоскости XY
        public static void reflectionXY(List<Vector3> l)
        {
            float[,] transMatrix = new float[4, 4] {
                                                { 1, 0, 0, 0 },
                                                { 0, 1, 0, 0 },
                                                { 0, 0, -1, 0 },
                                                { 0, 0, 0, 1 }
                                              };
            AffineTransformationList(l, transMatrix);
        }

        //отражение относитльено плоскости YZ
        public static void reflectionYZ(List<Vector3> l)
        {
            float[,] transMatrix = new float[4, 4] {
                                                { -1, 0, 0, 0 },
                                                { 0, 1, 0, 0 },
                                                { 0, 0, 1, 0 },
                                                { 0, 0, 0, 1 }
                                              };
            AffineTransformationList(l, transMatrix);
        }

        //отражение относитльено плоскости XZ
        public static void reflectionXZ(List<Vector3> l)
        {
            float[,] transMatrix = new float[4, 4] {
                                                { 1, 0, 0, 0 },
                                                { 0, -1, 0, 0 },
                                                { 0, 0, 1, 0 },
                                                { 0, 0, 0, 1 }
                                              };
            AffineTransformationList(l, transMatrix);
        }

        //Поворот по оси X
        public static void RotationAroundX(List<Vector3> l, float fi)
        {
            fi = fi * (float)Math.PI / 180;
            float[,] transMatrix = new float[4, 4] {
                                                { 1, 0, 0, 0 },
                                                { 0, (float)Math.Cos(fi), (float)Math.Sin(fi), 0 },
                                                { 0, -(float)Math.Sin(fi), (float)Math.Cos(fi), 0 },
                                                { 0, 0, 0, 1 }
                                              };
            AffineTransformationList(l, transMatrix);
        }

        //Поворот по оси Y
        public static void RotationAroundY(List<Vector3> l, float fi)
        {
            fi = fi * (float)Math.PI / 180;
            float[,] transMatrix = new float[4, 4] {
                                                { (float)Math.Cos(fi), 0,  -(float)Math.Sin(fi), 0 },
                                                { 0, 1, 0, 0 },
                                                { (float)Math.Sin(fi), 0, (float)Math.Cos(fi), 0 },
                                                { 0, 0, 0, 1 }
                                              };
            AffineTransformationList(l, transMatrix);
        }

        //Поворот по оси Z
        public static void RotationAroundZ(List<Vector3> l, float fi)
        {
            fi = fi * (float)Math.PI / 180;
            float[,] transMatrix = new float[4, 4] {
                                                { (float)Math.Cos(fi), (float)Math.Sin(fi), 0, 0 },
                                                { -(float)Math.Sin(fi), (float)Math.Cos(fi), 0, 0 },
                                                { 0, 0, 1, 0 },
                                                { 0, 0, 0, 1 }
                                              };
            AffineTransformationList(l, transMatrix);
        }

        //вращение вокруг прямой паралельной X и проходящей через точку
        public static void RotationAroundLineParallelXPassingPoint(List<Vector3> l, float fi, float x, float y, float z)
        {
            Offset(l, -x, -y, -z);
            RotationAroundX(l, fi);
            Offset(l, x, y, z);
        }

        //вращение вокруг прямой паралельной Y и проходящей через точку
        public static void RotationAroundLineParallelYPassingPoint(List<Vector3> l, float fi, float x, float y, float z)
        {
            Offset(l, -x, -y, -z);
            RotationAroundY(l, fi);
            Offset(l, x, y, z);
        }

        //вращение вокруг прямой паралельной Z и проходящей через точку
        public static void RotationAroundLineParallelZPassingPoint(List<Vector3> l, float fi, float x, float y, float z)
        {
            Offset(l, -x, -y, -z);
            RotationAroundZ(l, fi);
            Offset(l, x, y, z);
        }

        //поворот вокруг произвольной прямой 
        public static void RotationAroundStraight(List<Vector3> l, float fi, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float[] normallDirectionVector = new float[3] { x2 - x1, y2 - y1, z2 - z1 };
            Vector.Normalization(normallDirectionVector);
            Offset(l, -x1, -y1, -z1);
            //l m n
            float d = MathF.Sqrt(normallDirectionVector[1] * normallDirectionVector[1] + normallDirectionVector[2] * normallDirectionVector[2]);
            float[,] m = new float[4, 4]
            {
                { 1, 0, 0, 0 },
                { 0, normallDirectionVector[2]/d, normallDirectionVector[1]/d, 0 },
                { 0, -normallDirectionVector[1]/d, normallDirectionVector[2]/d, 0 },
                { 0, 0, 0, 1}
            };
            AffineTransformationList(l, m);
            float[,] m2 = new float[4, 4]
            {
                { 1, 0, d, 0},
                { 0, 1, 0, 0},
                { -d, 0, normallDirectionVector[0], 0 },
                { 0, 0, 0, normallDirectionVector[0]}
            };
            RotationAroundZ(l, fi);

            m = new float[4, 4]
            {
                { 1, 0, 0, 0 },
                { 0, normallDirectionVector[2]/d, -normallDirectionVector[1]/d, 0 },
                { 0, normallDirectionVector[1]/d, normallDirectionVector[2]/d, 0 },
                { 0, 0, 0, 1}
            };
            AffineTransformationList(l, m);

            m2 = new float[4, 4]
            {
                { 1, 0, -d, 0},
                { 0, 1, 0, 0},
                { d, 0, normallDirectionVector[0], 0 },
                { 0, 0, 0, normallDirectionVector[0]}
            };
            Offset(l, x1, y1, z1);
        }

    }
}
