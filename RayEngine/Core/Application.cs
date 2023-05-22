using RayEngine.Debug;
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
            using var _it = Profiler.Function();

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
            using var _itRun = Profiler.Function();

            Running = true;

            while (Running)
            {
                using var _itRunLoop = Profiler.Scope("RunLoop");

                double time = Time.GetTime();
                Timestep ts = time - LastFrameTime;
                LastFrameTime = time;

                using (var _itUpdate = Profiler.Scope("Update"))
                {
                    SceneManager.GetScene()?.OnUpdate(ts);
                }

                using (var _itRaylibRender = Profiler.Scope("Render"))
                {
                    Renderer.Begin();

                    SceneManager.GetScene()?.OnRender();

                    Raylib.DrawText(Raylib.GetFPS().ToString() + " FPS", 10, 10, 24, Color.RAYWHITE);
                    Raylib.DrawText($"{(double)ts:0.000} Deltatime", 10, 30, 24, Color.RAYWHITE);

                    Renderer.End();
                }

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
