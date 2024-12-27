using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTraycing
{
    public class Matrix
    {
        public static float[,] Multiplication(float[,] m1, float[,] m2)
        {
            int row1 = m1.GetLength(0);
            int col1 = m1.GetLength(1);
            int row2 = m2.GetLength(0);
            int col2 = m2.GetLength(1);
            float[,] res = new float[row1, col2];
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col2; j++)
                {
                    float cell = 0;
                    for (int k = 0; k < col1; k++)
                    {
                        cell += m1[i, k] * m2[k, j];
                    }
                    res[i, j] = cell;
                }
            }
            return res;
        }
    }
}
