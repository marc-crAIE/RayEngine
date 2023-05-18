using RayEngine.Core;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using RayEngine.Graphics;
using RayEngine.Scenes;
using SharpMaths;

namespace Sandbox
{
    internal class SandboxApp : Application
    {
        public SandboxApp(ApplicationSpecification specification) : base(specification)
        {
            Scene scene = new Scene();
            SceneManager.LoadScene(scene);

            GameObject parent = new GameObject();
            GameObject child1 = new GameObject();
            GameObject child2 = new GameObject();

            child1.SetParent(ref parent);
            child2.SetParent(ref child1);

            parent.AddComponent<SpriteComponent>(new Colour(255, 0, 255, 255));
            parent.GetComponent<TransformComponent>().Translation = new Vector3(10, 10, 0);
            parent.GetComponent<TransformComponent>().Scale = new Vector3(30.0f);

            parent.AddComponent<ScriptComponent>().Bind<ParentScript>();

            var child1Transform = child1.GetComponent<TransformComponent>();
            child1Transform.Translation = new Vector3(5, 7.5f, 0.0f);
            child1Transform.Scale = new Vector3(1.0f);

            var child2Transform = child2.GetComponent<TransformComponent>();
            child2Transform.Translation = new Vector3(2, 2, 0.0f);
            child2Transform.Rotation = SharpMath.ToRadians(new Vector3(0.0f, 0.0f, 45.0f));
            child2Transform.Scale = new Vector3(1.0f);

            child1.AddComponent<SpriteComponent>(new Colour(255, 0, 0, 255));
            child1.AddComponent<ScriptComponent>().Bind<ChildScript>();

            child2.AddComponent<SpriteComponent>(new Colour(0, 0, 255, 255));
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