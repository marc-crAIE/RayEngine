using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.GameObjects.Components
{
    public class ScriptComponent
    {
        public ScriptableGameObject? Instance { get; private set; }

        public void Bind<T>(params object[] args) where T : ScriptableGameObject
        {
            Type type = typeof(T);
            Type[] argTypes = new Type[args.Length];
            for (int i = 0; i < argTypes.Length; i++)
                argTypes[i] = args[i].GetType();
            ConstructorInfo? ctor = type.GetConstructor(argTypes);

            if (ctor == null)
                throw new ArgumentException("Invalid arguments to call constructor method for Component");

            Instance = (T)ctor.Invoke(args);
        }
    }
}
