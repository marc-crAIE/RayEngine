using RayEngine.Core;
using RayEngine.Debug;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using RayEngine.Graphics;
using Raylib_cs;
using SharpECS;
using SharpMaths;

namespace RayEngine.Scenes
{
    public class Scene
    {
        internal Dictionary<ulong, GameObject> GameObjects = new Dictionary<ulong, GameObject>();
        private EntityRegistry Registry = new EntityRegistry();

        private Layers Layers = new Layers();

        public delegate void OnImGUIRenderFunc();
        public OnImGUIRenderFunc? OnImGUIRender { internal get; set; }

        public void OnUpdate(Timestep ts)
        {
            using var _itSceneUpdate = Profiler.Function();

            UpdateGameObjects(ts);
        }

        public void OnRender()
        {
            using var _itSceneRender = Profiler.Function();

            RenderGameObjects();
        }

        private void UpdateGameObjects(Timestep ts)
        {
            using var _it = Profiler.Function();

            foreach (GameObject gameObject in GameObjects.Values)
            {
                if (gameObject.HasComponent<ScriptComponent>() && Layers.IsLayerEnabled(gameObject.LayerID))
                {
                    UUID id = Registry.Get<UUID>(gameObject.EntityHandle);
                    ScriptableGameObject? script = Registry.Get<ScriptComponent>(gameObject.EntityHandle).Instance;
                    if (script is null)
                        continue;
                    if (script.GameObject is null)
                    {
                        script.GameObject = GameObjects[id];
                        script.OnCreate();
                    }
                    script.OnUpdate(ts);
                }

                UpdateGameObjectChildren(gameObject, ts);
            }
        }

        private void UpdateGameObjectChildren(GameObject gameObject, Timestep ts)
        {
            using var _it = Profiler.Function();

            if (gameObject.Registry is null)
                return;

            foreach (GameObject child in gameObject.Children.Values)
            {
                if (child.HasComponent<ScriptComponent>() && Layers.IsLayerEnabled(gameObject.LayerID))
                {
                    UUID id = gameObject.Registry.Get<UUID>(child.EntityHandle);
                    ScriptableGameObject? script = gameObject.Registry.Get<ScriptComponent>(child.EntityHandle).Instance;
                    if (script is null)
                        continue;
                    if (script.GameObject is null)
                    {
                        script.GameObject = gameObject.Children[id];
                        script.OnCreate();
                    }
                    script.OnUpdate(ts);
                }

                UpdateGameObjectChildren(child, ts);
            }
        }

        private void RenderGameObjects()
        {
            using var _it = Profiler.Function();

            foreach (GameObject gameObject in GameObjects.Values)
            {
                Matrix4 transform = Registry.Get<TransformComponent>(gameObject.EntityHandle);
                if (gameObject.HasComponent<SpriteComponent>() && Layers.IsLayerEnabled(gameObject.LayerID))
                {
                    Renderer2D.DrawQuad(transform, Registry.Get<SpriteComponent>(gameObject.EntityHandle).Colour);
                }
                RenderGameObjectChildren(gameObject, transform);
            }
        }

        private void RenderGameObjectChildren(GameObject gameObject, Matrix4 transform)
        {
            using var _it = Profiler.Function();

            if (gameObject.Registry is null)
                return;

            foreach (GameObject child in gameObject.Children.Values)
            {
                Matrix4 childTransform = transform * gameObject.Registry.Get<TransformComponent>(child.EntityHandle);
                if (gameObject.HasComponent<SpriteComponent>() && Layers.IsLayerEnabled(gameObject.LayerID))
                {
                    Renderer2D.DrawSprite(childTransform, gameObject.Registry.Get<SpriteComponent>(child.EntityHandle));
                }
                RenderGameObjectChildren(child, childTransform);
            }
        }

        public ref Layers GetLayers() => ref Layers;

        internal ref EntityRegistry GetRegistry() => ref Registry;
    }
}
