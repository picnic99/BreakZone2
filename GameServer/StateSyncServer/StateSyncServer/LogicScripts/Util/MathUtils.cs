using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Util
{
    class MathUtils
    {




        // 计算两个向量之间的夹角（弧度）  
        public static double AngleBetween(Vector3 v1, Vector3 v2)
        {
            double dot = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
            double det = Math.Sqrt((v1.X * v1.X + v1.Y * v1.Y + v1.Z * v1.Z) *
                                     (v2.X * v2.X + v2.Y * v2.Y + v2.Z * v2.Z));
            if (det == 0) return 0; // 避免除以零  
            return Math.Acos(dot / det);
        }

        // 计算从B到A的方向向量相对于(0,0,1)方向旋转了多少度  
        public static float RotationAngleFromZAxis(Vector3 b, Vector3 a)
        {
            // 计算从B到A的方向向量  
            Vector3 direction = new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
            // 归一化方向向量  
            direction = direction.Normalize();

            // 参考方向是(0,0,1)  
            Vector3 reference = new Vector3(0, 0, 1);

            // 计算夹角（弧度）  
            double angleRadians = AngleBetween(reference, direction);

            // 使用叉积确定旋转方向（顺时针或逆时针）  
            Vector3 cross = new Vector3(
                reference.Y * direction.Z - reference.Z * direction.Y,
                reference.Z * direction.X - reference.X * direction.Z,
                reference.X * direction.Y - reference.Y * direction.X);
            if (cross.X < 0)
            {
                angleRadians = -angleRadians; // 如果需要逆时针为正，调整符号  
            }

            // 将弧度转换为度  
            double angleDegrees = angleRadians * (180.0 / Math.PI);

            // 确保角度在0-360度之间  
            angleDegrees = (angleDegrees + 360) % 360;

            return (float)angleDegrees;
        }


        // 计算两个单位向量之间的夹角（0-360度）  
        public static double CalculateAngleBetweenUnitVectors(double x1, double y1, double x2, double y2)
        {
            // 计算每个向量相对于x轴的角度（弧度）  
            double angleRadians1 = Math.Atan2(y1, x1);
            double angleRadians2 = Math.Atan2(y2, x2);

            // 计算两个角度之间的差值（弧度）  
            double angleDifferenceRadians = angleRadians2 - angleRadians1;

            // 将差值转换为0-2π范围（如果需要的话）  
            while (angleDifferenceRadians < 0)
            {
                angleDifferenceRadians += 2 * Math.PI;
            }
            while (angleDifferenceRadians >= 2 * Math.PI)
            {
                angleDifferenceRadians -= 2 * Math.PI;
            }

            // 将弧度转换为度  
            double angleDegrees = angleDifferenceRadians * (180.0 / Math.PI);

            // 确保角度在0-360度之间  
            angleDegrees = angleDegrees % 360;
            if (angleDegrees < 0)
            {
                angleDegrees += 360;
            }

            return angleDegrees;
        }



        // 计算两个单位向量之间的夹角（0-360度，顺时针）  
        public static float CalculateAngleBetweenUnitVectors(Vector3 a, Vector3 b , Vector3 around)
        {

            Vector3 v1 = around;

            Vector3 v2 = Vector3.Normalize(a - b);

            // 确保是单位向量  
            // 这里假设v1和v2已经是单位向量，如果不是，需要先进行归一化  

            // 计算点积  
            double dotProduct = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

            // 计算叉积的y分量（用于确定旋转方向）  
            double crossProductY = v1.Z * v2.X - v1.X * v2.Z;

            // 使用atan2函数计算角度，注意atan2的参数顺序影响结果的正负号  
            double angleRadians = Math.Atan2(crossProductY, dotProduct);

            // 将角度转换为0-360度范围（顺时针）  
            double angleDegrees = angleRadians * (180.0 / Math.PI);
            if (angleDegrees < 0)
            {
                angleDegrees += 360; // 如果角度是负数，则加上360度使其变为正数  
            }

            return (float)angleDegrees;
        }


        public static Quaternion LookRotation(Vector3 forward, Vector3 up)
        {
            // 确保forward是单位向量  
            Vector3 normalizedForward = Vector3.Normalize(forward);

            // 计算右向量（通过forward和up的叉积）  
            Vector3 right = Vector3.Cross(up, normalizedForward);

            // 如果forward和up太接近平行，则right将是接近零的向量，这时我们需要选择一个替代的up向量  
            if (Vector3.Dot(right, right) < float.Epsilon)
            {
                if (Math.Abs(Vector3.Dot(forward, Vector3.UnitY)) > 0.9999f)
                    right = Vector3.Cross(forward, Vector3.UnitX);
                else
                    right = Vector3.Cross(forward, Vector3.UnitY);
            }

            // 再次计算up向量（现在通过right和forward的叉积）  
            Vector3 calculatedUp = Vector3.Cross(normalizedForward, right);

            // 构建旋转矩阵的列  
            Vector3 xAxis = right;
            Vector3 yAxis = calculatedUp;
            Vector3 zAxis = normalizedForward;

            // 根据旋转矩阵创建四元数  
            float trace = xAxis.X + yAxis.Y + zAxis.Z + 1.0f;
            if (trace > 0.00001f)
            {
                float s = 0.5f /(float) Math.Sqrt(trace);
                return new Quaternion(
                    (yAxis.Z - zAxis.Y) * s,
                    (zAxis.X - xAxis.Z) * s,
                    (xAxis.Y - yAxis.X) * s,
                    0.25f * trace * s);
            }
            else if ((xAxis.X > yAxis.Y) && (xAxis.X > zAxis.Z))
            {
                float s = 2.0f * (float)Math.Sqrt(1.0f + xAxis.X - yAxis.Y - zAxis.Z);
                return new Quaternion(
                    0.25f * s,
                    (yAxis.Z + zAxis.Y) / s,
                    (zAxis.X + xAxis.Z) / s,
                    (xAxis.Y - yAxis.X) / s);
            }
            else if (yAxis.Y > zAxis.Z)
            {
                float s = 2.0f * (float)Math.Sqrt(1.0f + yAxis.Y - xAxis.X - zAxis.Z);
                return new Quaternion(
                    (zAxis.Y + yAxis.Z) / s,
                    0.25f * s,
                    (xAxis.Y + yAxis.X) / s,
                    (zAxis.X - xAxis.Z) / s);
            }
            else
            {
                float s = 2.0f * (float)Math.Sqrt(1.0f + zAxis.Z - xAxis.X - yAxis.Y);
                return new Quaternion(
                    (xAxis.Z + zAxis.X) / s,
                    (yAxis.X + xAxis.Y) / s,
                    0.25f * s,
                    (yAxis.Z - zAxis.Y) / s);
            }
        }
        public static Matrix4x4 ToRotationMatrix(Quaternion quaternion)
        {
            // 从四元数中提取分量  
            float x = quaternion.X;
            float y = quaternion.Y;
            float z = quaternion.Z;
            float w = quaternion.W;

            // 计算旋转矩阵的各个元素  
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;
            float wx = w * x;
            float wy = w * y;
            float wz = w * z;

            // 构建旋转矩阵  
            Matrix4x4 rotationMatrix = new Matrix4x4(
                1 - 2 * (y2 + z2), 2 * (xy + wz), 2 * (xz - wy), 0,
                2 * (xy - wz), 1 - 2 * (x2 + z2), 2 * (yz + wx), 0,
                2 * (xz + wy), 2 * (yz - wx), 1 - 2 * (x2 + y2), 0,
                0, 0, 0, 1
            );

            return rotationMatrix;
        }
    }

}
