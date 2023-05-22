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

            Entity[] scriptEntities = Registry.GetEntities().With<ScriptComponent>().AsArray();
            foreach (Entity entity in scriptEntities)
            {
                UUID id = Registry.Get<UUID>(entity);
                ScriptableGameObject? script = Registry.Get<ScriptComponent>(entity).Instance;
                if (script is null)
                    continue;
                if (script.GameObject is null)
                {
                    script.GameObject = GameObjects[id];
                    script.OnCreate();
                }
                script.OnUpdate(ts);
                UpdateGameObjectChildren(GameObjects[id], ts);
            }
        }

        private void UpdateGameObjectChildren(GameObject gameObject, Timestep ts)
        {
            using var _it = Profiler.Function();

            if (gameObject.Registry is null)
                return;

            Entity[] scriptEntities = gameObject.Registry.GetEntities().With<ScriptComponent>().AsArray();
            foreach (Entity entity in scriptEntities)
            {
                UUID id = gameObject.Registry.Get<UUID>(entity);
                ScriptableGameObject? script = gameObject.Registry.Get<ScriptComponent>(entity).Instance;
                if (script is null)
                    continue;
                if (script.GameObject is null)
                {
                    script.GameObject = gameObject.Children[id];
                    script.OnCreate();
                }
                script.OnUpdate(ts);

                UpdateGameObjectChildren(gameObject.Children[id], ts);
            }
        }

        private void RenderGameObjects()
        {
            using var _it = Profiler.Function();

            Entity[] entities = Registry.GetEntities().With<SpriteComponent>().AsArray();
            foreach (Entity entity in entities)
            {
                Matrix4 transform = Registry.Get<TransformComponent>(entity);
                Renderer2D.DrawQuad(transform, Registry.Get<SpriteComponent>(entity).Colour);
                RenderGameObjectChildren(GameObjects[Registry.Get<UUID>(entity)], transform);
            }
        }

        private void RenderGameObjectChildren(GameObject gameObject, Matrix4 transform)
        {
            using var _it = Profiler.Function();

            if (gameObject.Registry is null)
                return;

            Entity[] entities = gameObject.Registry.GetEntities().With<SpriteComponent>().AsArray();
            foreach (Entity entity in entities)
            {
                Matrix4 childTransform = transform * gameObject.Registry.Get<TransformComponent>(entity);
                Renderer2D.DrawSprite(childTransform, gameObject.Registry.Get<SpriteComponent>(entity));
                RenderGameObjectChildren(gameObject.Children[gameObject.Registry.Get<UUID>(entity)], childTransform);
            }
        }

        internal ref EntityRegistry GetRegistry() => ref Registry;
    }
}
