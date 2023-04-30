using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Events
{
    public class WindowResizeEvent : Event
    {
        private readonly int Width, Height;

        public WindowResizeEvent(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int GetWidth() => Width;
        public int GetHeight() => Height;

        public override string ToString() => $"WindowResizeEvent: {Width}, {Height}";

        public override EventType GetEventType() => EventType.WindowResize;

        public override string GetName() => "WindowResize";

        public override int GetCategoryFlags() => (int)EventCategory.Application;
    }

    public class WindowCloseEvent : Event
    {
        public override EventType GetEventType() => EventType.WindowClose;
        public override string GetName() => "WindowClose";
        public override int GetCategoryFlags() => (int)EventCategory.Application;
    }
}
