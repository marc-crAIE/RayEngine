using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Events
{
    public enum EventType
    {
        None = 0,
        WindowClose, WindowResize, WindowFocus, WindowLostFocus, WindowMoved,
        AppTick, AppUpdate, AppRender,
        KeyPressed, KeyReleased, KeyTyped,
        MouseButtonPressed, MouseButtonReleased, MouseMoved, MouseScrolled
    }

    public enum EventCategory
    {
        None = 0,
        Application = (1 << 0),
        Input = (1 << 1),
        Keyboard = (1 << 2),
        Mouse = (1 << 3),
        MouseButton = (1 << 4),
    }

    public abstract class Event
    {
        public bool Handled = false;

        public abstract EventType GetEventType();
        public abstract string GetName();
        public abstract int GetCategoryFlags();

        public override string ToString() => GetName();

        public bool IsInCategory(EventCategory category) => (GetCategoryFlags() & (int)category) != 0;
    }

    public static class EventDispatcher
    {
        public static bool Dispatch<T>(ref Event e, Func<T, bool> func) where T : Event
        {
            if (e.GetType() == typeof(T))
            {
                e.Handled = func((T)e);
                return true;
            }
            return false;
        }
    }
}
