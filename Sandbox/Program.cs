using RayEngine.Core;
using RayEngine.GameObjects;

namespace Sandbox
{
    internal class SandboxApp : Application
    {
        public SandboxApp(ApplicationSpecification specification) : base(specification)
        {
            GameObject parent = new GameObject();
            GameObject child1 = new GameObject();
            GameObject child2 = new GameObject();

            child1.SetParent(ref parent);
            child2.SetParent(ref parent);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ApplicationSpecification spec = new ApplicationSpecification()
            {
                Name = "Sandbox App"
            };
            SandboxApp app = new SandboxApp(spec);
            app.Run();
        }
    }
}