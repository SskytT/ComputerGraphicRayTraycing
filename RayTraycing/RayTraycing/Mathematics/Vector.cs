using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraycing
{
    public class Vector
    {
        public static float Length(float[] vector)
        {
            float res = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                res += vector[i] * vector[i];
            }
            return MathF.Sqrt(res);
        }

        public static void Normalization(float[] vector)
        {
            float length = Length(vector);
            if (length == 0)
                return;
            for (int i = 0; i < vector.Length; i++)
                vector[i] /= length;
        }

        public static float[] Multiplication(float[] v1, float[] v2)
        {
            float[] result = new float[v1.Length];
            result[0] = v1[1] * v2[2] - v1[2] * v2[1];
            result[1] = v1[2] * v2[0] - v1[0] * v2[2];
            result[2] = v1[0] * v2[1] - v1[1] * v2[0];
            return result;
        }

        public static float ScalarProduct(float[] v1, float[] v2)
        {
            return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
        }
    }
}
