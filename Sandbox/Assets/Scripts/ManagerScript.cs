using RayEngine.Core;
using RayEngine.GameObjects;

namespace Sandbox.Assets.Scripts
{
    internal class ManagerScript : ScriptableGameObject
    {
        private Layer ImGUILayer;

        public override void OnCreate()
        {
            ImGUILayer = Scene.GetLayers().GetLayer("ImGUI");
            ImGUILayer.Enable();
        }

        public override void OnUpdate(Timestep ts)
        {
            if (Input.IsKeyTyped(Raylib_cs.KeyboardKey.KEY_F1))
                ImGUILayer.Enable(!ImGUILayer.IsEnabled());
        }
    }
}
