using RayEngine.Core;
using RayEngine.GameObjects.Components;
using RayEngine.Scenes;
using SharpMaths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.GameObjects
{
    public class ScriptableGameObject
    {
        internal GameObject? GameObject;

        #region Properties

        protected GameObject Self { get => GameObject; }

        protected Scene Scene { get => GameObject.Scene; }

        protected TransformComponent Transform
        {
            get
            {
                return GetComponent<TransformComponent>();
            }
        }

        #endregion

        #region Component Functions

        protected ref T AddComponent<T>(params object[] args)
        {
            if (GameObject is null)
                throw new InvalidOperationException("GameObject is null");

            return ref GameObject.AddComponent<T>(args);
        }

        protected bool RemoveComponent<T>()
        {
            if (GameObject is null)
                throw new InvalidOperationException("GameObject is null");

            return GameObject.RemoveComponent<T>();
        }

        protected ref T GetComponent<T>() 
        {
            if (GameObject is null)
                throw new InvalidOperationException("GameObject is null");

            return ref GameObject.GetComponent<T>();
        }

        protected bool HasComponent<T>()
        {
            if (GameObject is null)
                throw new InvalidOperationException("GameObject is null");

            return GameObject.HasComponent<T>();
        }

        #endregion

        public virtual void OnCreate() { }
        public virtual void OnDestroy() { }
        public virtual void OnUpdate(Timestep ts) { }
    }
}
