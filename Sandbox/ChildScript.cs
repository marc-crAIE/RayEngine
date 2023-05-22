using RayEngine.Core;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using SharpMaths;

namespace Sandbox
{
    internal class ChildScript : ScriptableGameObject
    {
        private float RotationAngle = 0.0f;
        private TransformComponent _Transform;

        public override void OnCreate()
        {
            Console.WriteLine("I am a child!");
            _Transform = Transform;
        }

        public override void OnUpdate(Timestep ts)
        {
            if (Input.IsKeyPressed(Key.KEY_R))
            {
                _Transform.Rotation = new Vector3(0.0f, 0.0f, RotationAngle += 2.0f * ts);
            }
        }
    }
}
