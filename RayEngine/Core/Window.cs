using RayEngine.Events;
using Raylib_cs;

namespace RayEngine.Core
{
    public struct WindowProps
    {
        public string Title;
        public int Width, Height;

        public WindowProps(string title = "RayEngine", int width = 800, int height = 600)
        {
            Title = title;
            Width = width;
            Height = height;
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
            Data = new WindowData();
            Data.Title = props.Title;
            Data.Width = props.Width;
            Data.Height = props.Height;

            Raylib.InitWindow(props.Width, props.Height, props.Title);
            Raylib.SetTargetFPS(60);

            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        }

        public int GetWidth() => Data.Width;
        public int GetHeight() => Data.Height;

        internal void OnUpdate()
        {
            if (Raylib.WindowShouldClose())
            {
                WindowCloseEvent e = new WindowCloseEvent();
                Data.EventCallback(e);
            }

            if (Raylib.IsWindowResized())
            {
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
