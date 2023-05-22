using RayEngine.Core;
using RayEngine.GameObjects.Components;
using RayEngine.Scenes;
using SharpECS;
using SharpMaths;
using System.Reflection;

namespace RayEngine.GameObjects
{
    public class GameObject
    {
        private UUID ID = new UUID();
        public GameObject? Parent { get; private set; } = null;
        public Dictionary<ulong, GameObject> Children { get; private set; } = new Dictionary<ulong, GameObject>();
        internal EntityRegistry? Registry; // Keeps track of children entities

        internal Entity EntityHandle = 0;
        private Scene? Scene = null;

        public GameObject(string tag = "GameObject")
        {
            if (SceneManager.GetScene() is null)
                throw new InvalidOperationException("No scene is currently loaded to add the GameObject to");

            Scene = SceneManager.GetScene();
            EntityHandle = Scene?.GetRegistry().Create() ?? 0;

            AddComponent<UUID>((ulong)ID);
            AddComponent<TagComponent>(tag);
            AddComponent<TransformComponent>();

            Scene?.GameObjects.Add(GetID(), this);
        }

        public void SetParent(ref GameObject parent)
        {
            Scene?.GameObjects.Remove(ID);
            Parent = parent;
            Parent.Children.Add(ID, this);

            parent.Registry ??= new EntityRegistry();

            Entity? newHandle = Scene?.GetRegistry().CopyTo(EntityHandle, parent.Registry);
            Scene?.GetRegistry().Destroy(EntityHandle);

            if (newHandle is not null)
                EntityHandle = newHandle;
        }

        public void AddChild(ref GameObject child)
        {
            Scene?.GameObjects.Remove(child.ID);
            Children.Add(child.ID, child);
            child.Parent = this;

            Registry ??= new EntityRegistry();

            Entity? newHandle = Scene?.GetRegistry().CopyTo(child.EntityHandle, Registry);
            Scene?.GetRegistry().Destroy(child.EntityHandle);

            if (newHandle is not null)
                child.EntityHandle = newHandle;
        }

        public UUID GetID() => GetComponent<UUID>();
        public string GetTag() => GetComponent<TagComponent>();

        public ref T AddComponent<T>(params object[] args)
        {
            if (Scene == null)
                throw new InvalidOperationException("GameObject was not created within a Scene");

            if (Parent is not null && Parent.Registry is not null)
                return ref Parent.Registry.Emplace<T>(EntityHandle, args);
            return ref Scene.GetRegistry().Emplace<T>(EntityHandle, args);
        }

        public ref T AddComponent<T>(in T component)
        {
            if (Scene == null)
                throw new InvalidOperationException("GameObject was not created within a Scene");

            if (Parent is not null && Parent.Registry is not null)
                return ref Parent.Registry.Add<T>(EntityHandle, component);
            return ref Scene.GetRegistry().Add<T>(EntityHandle, component);
        }

        public bool RemoveComponent<T>()
        {
            if (Scene == null)
                throw new InvalidOperationException("GameObject was not created within a Scene");

            T? typeCheck = default(T);
            if (typeCheck is UUID || typeCheck is TagComponent || typeCheck is TransformComponent)
                throw new InvalidOperationException($"Cannot remove {typeof(T)} from GameObject");

            if (Parent is not null && Parent.Registry is not null)
                return Parent.Registry.Remove<T>(EntityHandle);
            return Scene.GetRegistry().Remove<T>(EntityHandle);
        }

        public ref T GetComponent<T>()
        {
            if (Scene == null)
                throw new InvalidOperationException("GameObject was not created within a Scene");

            if (Parent is not null && Parent.Registry is not null)
                return ref Parent.Registry.Get<T>(EntityHandle);
            return ref Scene.GetRegistry().Get<T>(EntityHandle);
        }

        public bool HasComponent<T>()
        {
            if (Scene == null)
                throw new InvalidOperationException("GameObject was not created within a Scene");

            if (Parent is not null && Parent.Registry is not null)
                return Parent.Registry.Has<T>(EntityHandle);
            return Scene.GetRegistry().Has<T>(EntityHandle);
        }

        internal bool SetScene(Scene scene)
        {
            if (Scene is not null)
                return false;
            Scene = scene;
            return true;
        }
    }
}
