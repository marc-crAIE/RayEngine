using RayEngine.Debug;
using RayEngine.Events;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using RayEngine.Graphics;
using RayEngine.ImGUI;
using RayEngine.Scenes;
using Raylib_cs;
using SharpMaths;

namespace RayEngine.Core
{
    public struct ApplicationSpecification
    {
        public string Name;
        public int Width, Height;

        public ApplicationSpecification() : this("RayEngine Application") { }

        public ApplicationSpecification(string name, int width = 1200, int height = 800)
        {
            Name = name;
            Width = width;
            Height = height;
        }
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

            AppWindow = new Window(new WindowProps(specification.Name, specification.Width, specification.Height));
            AppWindow.SetEventCallback(OnEvent);

            ImGuiContext.Setup();
        }

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

                Scene? scene = SceneManager.GetScene();

                using (var _itUpdate = Profiler.Scope("Update"))
                {
                    scene?.OnUpdate(ts);
                }

                using (var _itRaylibRender = Profiler.Scope("Render"))
                {
                    Renderer.Begin();

                    scene?.OnRender();

                    Raylib.DrawText(Raylib.GetFPS().ToString() + " FPS", 10, 10, 24, Color.RAYWHITE);
                    Raylib.DrawText($"{(double)ts:0.000} Deltatime", 10, 30, 24, Color.RAYWHITE);

                    ImGuiContext.Begin();

                    if (scene?.OnImGUIRender is not null && scene.GetLayers().IsLayerEnabled("ImGUI"))
                        scene.OnImGUIRender();

                    //ImGuiNET.ImGui.ShowDemoWindow();

                    ImGuiContext.End();

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
