using RayEngine.Core;
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

        public virtual void OnCreate() { }
        public virtual void OnDestroy() { }
        public virtual void OnUpdate(Timestep ts) { }
    }
}
