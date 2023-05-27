using SharpMaths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Utils
{
    public sealed class RMath
    {
        public static Random Rand { get; } = new Random();

        public static Tuple<Vector3, Vector3, Vector3> GetTransformRotationScale(Matrix4 transform)
        {
            Vector3 scale = new Vector3(
                ((Vector3)transform[0]).Magnitude(),
                ((Vector3)transform[1]).Magnitude(),
                ((Vector3)transform[2]).Magnitude());

            Matrix3 rm = (Matrix3)transform;
            rm[0] /= scale.x != 0 ? scale.x : 1.0f;
            rm[1] /= scale.y != 0 ? scale.y : 1.0f;
            rm[2] /= scale.z != 0 ? scale.z : 1.0f;
                        
            Vector3 rotation = GetEulerFromRotationMatrix(rm);

            Vector3 translation = transform[3];

            return new Tuple<Vector3, Vector3, Vector3>(translation, rotation, scale);
        }

        public static Vector3 GetEulerFromRotationMatrix(Matrix3 rm)
        {
            float rx, ry, rz;

            if (rm[0, 2] != Math.Abs(1))
            {
                float pitch1 = -(float)Math.Asin(rm[0, 2]);
                float pitch2 = (float)Math.PI - pitch1;
                float cosPitch1 = (float)Math.Cos(pitch1);
                float cosPitch2 = (float)Math.Cos(pitch2);
                float roll1 = (float)Math.Atan2(rm[1, 2] / cosPitch1, rm[2, 2] / cosPitch1);
                float roll2 = (float)Math.Atan2(rm[1, 2] / cosPitch2, rm[2, 2] / cosPitch2);
                float yaw1 = (float)Math.Atan2(rm[0, 1] / cosPitch1, rm[0, 0] / cosPitch1);
                float yaw2 = (float)Math.Atan2(rm[0, 1] / cosPitch2, rm[0, 0] / cosPitch2);

                rx = roll1;
                ry = pitch1;
                rz = yaw1;
            }
            else
            {
                rz = 0;
                if (rm[2, 0] == -1)
                {
                    rx = rz + (float)Math.Atan2(rm[1, 0], rm[2, 0]);
                    ry = (float)Math.PI / 2;
                }
                else
                {
                    rx = -rz + (float)Math.Atan2(-rm[1, 0], -rm[2, 0]);
                    ry = -(float)Math.PI / 2;
                }
            }

            return new Vector3(rx, ry, rz);
        }

        public static Quaternion GetQuatFromRotationMatrix(Matrix3 rm)
        {
            Quaternion quat = new Quaternion();
            float tr = rm[0, 0] + rm[1, 1] + rm[2, 2];
            if (tr > 0)
            {
                float s = (float)Math.Sqrt(1 + tr) * 2;
                quat.w = 0.25f * s;
                quat.x = (rm[1, 2] - rm[2, 1]) / s;
                quat.y = (rm[2, 0] - rm[0, 2]) / s;
                quat.z = (rm[0, 1] - rm[1, 0]) / s;
            }
            else if ((rm[0, 0] > rm[1, 1]) && (rm[0, 0] > rm[2, 2]))
            {
                float s = (float)Math.Sqrt(1 + rm[0, 0] - rm[1, 1] - rm[2, 2]) * 2;
                quat.w = (rm[1, 2] - rm[2, 1]) / s;
                quat.x = 0.25f * s;
                quat.y = (rm[1, 0] + rm[0, 1]) / s;
                quat.z = (rm[2, 0] + rm[0, 2]) / s;
            }
            else if (rm[1, 1] > rm[2, 2])
            {
                float s = (float)Math.Sqrt(1 + rm[2, 2] - rm[0, 0] - rm[1, 1]) * 2;
                quat.w = (rm[2, 0] - rm[1, 0]) / s;
                quat.x = (rm[1, 0] + rm[0, 1]) / s;
                quat.y = 0.25f * s;
                quat.z = (rm[2, 1] + rm[1, 2]) / s;
            }
            else
            {
                float s = (float)Math.Sqrt(1 + rm[2, 2] - rm[0, 0] - rm[1, 1]) * 2;
                quat.w = (rm[0, 1] - rm[1, 0]) / s;
                quat.x = (rm[2, 0] + rm[0, 2]) / s;
                quat.y = (rm[2, 1] + rm[1, 2]) / s;
                quat.z = 0.25f * s;
            }

            return quat;
        }

        public static Vector3 GetEulerFromQuaterion(Quaternion quat)
        {
            Vector3 rotation = new Vector3();

            float sqw = quat.w * quat.w;
            float sqx = quat.x * quat.x;
            float sqy = quat.y * quat.y;
            float sqz = quat.z * quat.z;

            float unit = sqx + sqy + sqz + sqw;
            float test = quat.x * quat.y + quat.z * quat.w;
            if (test > 0.499f * unit)
            {
                rotation.x = 0;
                rotation.y = 2 * (float)Math.Atan2(quat.x, quat.w);
                rotation.z = (float)Math.PI / 2;
                return rotation;
            }
            else if (test < -0.499f * unit)
            {
                rotation.x = 0;
                rotation.y = -2 * (float)Math.Atan2(quat.x, quat.w);
                rotation.z = -(float)Math.PI / 2;
                return rotation;
            }
            
            rotation.x = (float)Math.Atan2(2 * quat.x * quat.w - 2 * quat.y * quat.z, -sqx + sqy - sqz + sqw);
            rotation.y = (float)Math.Atan2(2 * quat.y * quat.w - 2 * quat.x * quat.z, sqx - sqy - sqz + sqw);
            rotation.z = (float)Math.Asin(2 * test / unit);
            return rotation;
        }
    }
}
