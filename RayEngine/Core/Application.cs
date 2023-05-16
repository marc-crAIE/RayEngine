using RayEngine.Events;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using RayEngine.Graphics;
using RayEngine.Scenes;
using Raylib_cs;
using SharpMaths;

namespace RayEngine.Core
{
    public struct ApplicationSpecification
    {
        public string Name;
    }

    public abstract class Application
    {
        public static Application? Instance { get; private set; } = null;

        private ApplicationSpecification Specification;
        private Window AppWindow;
        private bool Running = false;
        private double LastFrameTime = 0.0;

        public Application(ApplicationSpecification specification)
        {
            if (Instance != null)
                throw new Exception("Application already exists!");

            Instance = this;
            Specification = specification;

            AppWindow = new Window(new WindowProps(specification.Name, 1200, 800));
            AppWindow.SetEventCallback(OnEvent);
        }

        float angle = 0.0f;

        public void Run()
        {
            Running = true;

            while (Running)
            {
                double time = Time.GetTime();
                Timestep ts = time - LastFrameTime;
                LastFrameTime = time;

                Renderer.Begin();

                SceneManager.GetScene()?.OnUpdate(ts);

                //Vector3 v = SharpMath.ToRadians(new Vector3(0.0f, 0.0f, angle += 1f));
                //Matrix4 rotation = new Quaternion(v);

                //Matrix4 m = Matrix4.Translation(200, 300, 0) * rotation * Matrix4.Scale(new Vector3(200.0f, 150.0f, 1.0f));
                //Matrix4 m2 = m * Matrix4.Translation(0.5f, 0.5f, 1f);

                //Renderer2D.DrawQuad(m, new Vector4(1.0f));
                //Renderer2D.DrawQuad(m2, new Vector4(1.0f, 0.0f, 0.0f, 1.0f));

                Raylib.DrawText(Raylib.GetFPS().ToString() + " FPS", 10, 10, 24, Color.RAYWHITE);

                Renderer.End();

                AppWindow.OnUpdate();
            }

            AppWindow.Close();
        }

        internal void OnEvent(Event e)
        {
            EventDispatcher.Dispatch<WindowCloseEvent>(ref e, OnWindowClose);
        }

        private bool OnWindowClose(WindowCloseEvent e)
        {
            Running = false;
            return true;
        }
    }
}
