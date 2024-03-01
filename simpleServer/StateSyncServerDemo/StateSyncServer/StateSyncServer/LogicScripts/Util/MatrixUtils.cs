using System;
using System.Numerics;

namespace StateSyncServer.LogicScripts.Util
{
    class MatrixUtils
    {
        public static void Test()
        {
            Matrix4x4 m = GetMoveMatrix(new Vector3(0, 0, 1f));
            Console.WriteLine(m.ToString());
            Console.WriteLine(MulMatrixVerctor(m, new Vector3(0, 0, 0)));
        }

        public static Matrix4x4 MulMatrix(Matrix4x4 a, Matrix4x4 b)
        {
            return a * b;
        }

        public static Vector3 MulMatrixVerctor(Matrix4x4 matrix, Vector3 vector)
        {
            // 将向量的W分量设置为1（齐次坐标）  
            Vector4 vector4 = new Vector4(vector.X, vector.Y, vector.Z, 1.0f);

            // 执行矩阵与向量的乘法  
            Vector4 result = new Vector4
            {
                X = matrix.M11 * vector4.X + matrix.M12 * vector4.Y + matrix.M13 * vector4.Z + matrix.M14 * vector4.W,
                Y = matrix.M21 * vector4.X + matrix.M22 * vector4.Y + matrix.M23 * vector4.Z + matrix.M24 * vector4.W,
                Z = matrix.M31 * vector4.X + matrix.M32 * vector4.Y + matrix.M33 * vector4.Z + matrix.M34 * vector4.W,
                W = matrix.M41 * vector4.X + matrix.M42 * vector4.Y + matrix.M43 * vector4.Z + matrix.M44 * vector4.W
            };

            // 如果需要，可以通过W分量对结果进行透视除法  
            // 如果W接近0，可能会导致数值不稳定，应当处理这种情况  
            if (Math.Abs(result.W) > float.Epsilon)
            {
                result /= result.W;
            }

            // 提取结果的X, Y, Z分量并返回  
            return new Vector3(result.X, result.Y, result.Z);
        }
        public static Matrix4x4 GetRotateYMatrix(float angle)
        {
            float cosPhi = (float)Math.Cos(angle * (Math.PI / 180));
            float sinPhi = (float)Math.Sin(angle * (Math.PI / 180));

            Matrix4x4 result = new Matrix4x4
            {
                M11 = cosPhi,
                M12 = 0,
                M13 = sinPhi,
                M14 = 0,
                M21 = 0,
                M22 = 1,
                M23 = 0,
                M24 = 0,
                M31 = -sinPhi,
                M32 = 0,
                M33 = cosPhi,
                M34 = 0,
                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };

            return result;
        }

        public static Matrix4x4 GetMoveMatrix(Vector3 pos)
        {
            Matrix4x4 result = new Matrix4x4
            {
                M11 = 1,
                M12 = 0,
                M13 = 0,
                M14 = pos.X,
                M21 = 0,
                M22 = 1,
                M23 = 0,
                M24 = pos.Y,
                M31 = 0,
                M32 = 0,
                M33 = 1,
                M34 = pos.Z,
                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1
            };
            return result;
        }
    }
}
