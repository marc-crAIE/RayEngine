using RayEngine.Core;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using RayEngine.Graphics;
using SharpECS;
using SharpMaths;

namespace RayEngine.Scenes
{
    public class Scene
    {
        internal Dictionary<UUID, GameObject> GameObjects = new Dictionary<UUID, GameObject>();
        private EntityRegistry Registry = new EntityRegistry();

        public void OnUpdate(Timestep ts)
        {
            Entity[] scriptEntities = Registry.GetEntities().With<ScriptComponent>().AsArray();
            foreach (Entity entity in scriptEntities)
            {
                ScriptableGameObject? script = Registry.Get<ScriptComponent>(entity).Instance;
                if (script is null)
                    continue;
                if (script.GameObject is null)
                {
                    script.GameObject = GameObjects[Registry.Get<UUID>(entity)];
                    script.OnCreate();
                }
                script.OnUpdate(ts);
            }

            Entity[] entities = Registry.GetEntities().With<TransformComponent>().With<SpriteComponent>().AsArray();
            foreach (Entity entity in entities)
            {
                Matrix4 transform = Registry.Get<TransformComponent>(entity);
                Renderer2D.DrawQuad(transform, Registry.Get<SpriteComponent>(entity).Colour);
            }


            foreach (GameObject obj in GameObjects.Values)
            {
                Matrix4 transform = obj.GetComponent<TransformComponent>();

                foreach (GameObject child in obj.Children)
                {
                    Matrix4 cTransform = transform * child.GetComponent<TransformComponent>();
                    Renderer2D.DrawQuad(cTransform, child.Parent.Registry.Get<SpriteComponent>(child.EntityHandle).Colour);
                }
            }
        }

        internal ref EntityRegistry GetRegistry() => ref Registry;
    }
}
