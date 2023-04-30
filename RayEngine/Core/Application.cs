using RayEngine.Events;
using RayEngine.Graphics;

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

        public Application(ApplicationSpecification specification)
        {
            if (Instance != null)
                throw new Exception("Application already exists!");

            Instance = this;
            Specification = specification;

            AppWindow = new Window(new WindowProps(specification.Name, 1200, 800));
            AppWindow.SetEventCallback(OnEvent);
        }

        public void Run()
        {
            Running = true;

            while (Running)
            {
                Renderer.Begin();

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
