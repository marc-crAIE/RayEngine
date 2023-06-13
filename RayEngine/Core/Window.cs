using RayEngine.Debug;
using RayEngine.Events;
using Raylib_cs;
using SharpMaths;

namespace RayEngine.Core
{
    public struct WindowProps
    {
        public string Title;
        public int Width, Height;
        public short FPSLimit;

        public WindowProps(string title = "RayEngine", int width = 800, int height = 600, short fpsLimit = short.MaxValue)
        {
            Title = title;
            Width = width;
            Height = height;
            FPSLimit = fpsLimit;
        }
    }

    public class Window
    {
        private struct WindowData
        {
            public string Title;
            public int Width, Height;
            public Action<Event> EventCallback;
        }
        private WindowData Data;

        public Window(WindowProps props)
        {
            using var _it = Profiler.Function();

            Data = new WindowData();
            Data.Title = props.Title;
            Data.Width = props.Width;
            Data.Height = props.Height;

            Raylib.InitWindow(props.Width, props.Height, props.Title);
            if (props.FPSLimit != short.MaxValue)
                Raylib.SetTargetFPS(props.FPSLimit);

            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        }

        public int GetWidth() => Data.Width;
        public int GetHeight() => Data.Height;

        public Vector2 GetSize() => new Vector2(GetHeight(), GetWidth());

        internal void OnUpdate()
        {
            using var _it = Profiler.Function();

            if (Raylib.WindowShouldClose())
            {
                using var _itClose = Profiler.Scope("Window Close");

                WindowCloseEvent e = new WindowCloseEvent();
                Data.EventCallback(e);
            }

            if (Raylib.IsWindowResized())
            {
                using var _itClose = Profiler.Scope("Window Resize");

                Data.Width = Raylib.GetScreenWidth();
                Data.Height = Raylib.GetScreenHeight();

                WindowResizeEvent e = new WindowResizeEvent(Data.Width, Data.Height);
                Data.EventCallback(e);
            }
        }

        internal void SetEventCallback(Action<Event> callback)
        {
            Data.EventCallback = callback;
        }

        internal void Close()
        {
            Raylib.CloseWindow();
        }
    }
}
