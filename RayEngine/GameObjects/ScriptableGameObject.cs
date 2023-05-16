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

        protected ref T GetComponent<T>() => ref GameObject.GetComponent<T>();

        public virtual void OnCreate() { }
        public virtual void OnDestroy() { }
        public virtual void OnUpdate(Timestep ts) { }
    }
}
