﻿using RayEngine.Core;
using RayEngine.GameObjects.Components;
using RayEngine.Scenes;
using SharpECS;

namespace RayEngine.GameObjects
{
    public class GameObject
    {
        public GameObject? Parent { get; private set; } = null;
        public List<GameObject> Children { get; private set; } = new List<GameObject>();
        internal EntityRegistry? Registry; // Keeps track of children entities

        internal Entity EntityHandle = 0;
        private Scene? Scene = null;

        public GameObject(string tag = "GameObject")
        {
            if (SceneManager.GetScene() is null)
                throw new InvalidOperationException("No scene is currently loaded to add the GameObject to");

            Scene = SceneManager.GetScene();
            EntityHandle = Scene?.GetRegistry().Create() ?? 0;

            AddComponent<UUID>();
            AddComponent<TagComponent>(tag);
            AddComponent<TransformComponent>();

            Scene?.GameObjects.Add(GetID(), this);
        }

        public void SetParent(ref GameObject parent)
        {
            Scene?.GameObjects.Remove(GetID());
            Parent = parent;
            Parent.Children.Add(this);

            parent.Registry ??= new EntityRegistry();

            Entity? newHandle = Scene?.GetRegistry().CopyTo(EntityHandle, parent.Registry);
            Scene?.GetRegistry().Destroy(EntityHandle);

            if (newHandle is not null)
                EntityHandle = newHandle;
        }

        public void AddChild(ref GameObject child)
        {
            Scene?.GameObjects.Remove(child.GetID());
            child.Parent = this;
            Children.Add(child);

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

        public bool RemoveComponent<T>()
        {
            if (Scene == null)
                throw new InvalidOperationException("GameObject was not created within a Scene");

            T typeCheck = default(T);
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

        internal bool SetScene(Scene scene)
        {
            if (Scene is not null)
                return false;
            Scene = scene;
            return true;
        }
    }
}
