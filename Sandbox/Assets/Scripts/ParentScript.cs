using RayEngine.Core;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using SharpMaths;

namespace Sandbox.Assets.Scripts
{
    internal class ParentScript : ScriptableGameObject
    {
        private float Rotation = 0.0f;
        private TransformComponent _Transform;

        public override void OnCreate()
        {
            Console.WriteLine("Hello!");
            _Transform = Transform;
        }

        public override void OnUpdate(Timestep ts)
        {
            _Transform.Translation = Input.GetMousePosition();

            if (Input.IsKeyPressed(Key.KEY_E))
                _Transform.Rotation = new Vector3(0.0f, 0.0f, Rotation += 1.0f * ts);
        }
    }
}
