using SharpMaths;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.GameObjects.Components
{
    public class TransformComponent
    {
        public Vector3 Translation = new Vector3();
        public Vector3 Rotation = new Vector3();
        public Vector3 Scale = new Vector3(10.0f);

        public Matrix4 GetTransform()
        {
            Matrix4 rotation = new Quaternion(Rotation);
            return Matrix4.Translation(Translation) * rotation * Matrix4.Scale(Scale);
        }

        public static implicit operator Matrix4(TransformComponent obj) => obj.GetTransform();
    }
}
