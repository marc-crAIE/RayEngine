using RayEngine.Core;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    internal class ParentScript : ScriptableGameObject
    {
        public override void OnCreate()
        {
            Console.WriteLine("Hello!");
        }

        public override void OnUpdate(Timestep ts)
        {
            GetComponent<TransformComponent>().Translation = Input.GetMousePosition();
        }
    }
}
