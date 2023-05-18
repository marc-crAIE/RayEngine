﻿using RayEngine.Core;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using SharpMaths;

namespace Sandbox
{
    internal class ParentScript : ScriptableGameObject
    {
        private float Rotation = 0.0f;
        private TransformComponent Transform;

        public override void OnCreate()
        {
            Console.WriteLine("Hello!");

            Transform = GetComponent<TransformComponent>();
        }

        public override void OnUpdate(Timestep ts)
        {
            Transform.Translation = Input.GetMousePosition();

            if (Input.IsKeyPressed(Key.KEY_E))
                Transform.Rotation = new Vector3(0.0f, 0.0f, Rotation += 1.0f * ts);
        }
    }
}
