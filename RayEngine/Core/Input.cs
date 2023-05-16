using Raylib_cs;
using SharpMaths;

namespace RayEngine.Core
{
    public static class Input
    {
        public static bool IsKeyPressed(KeyboardKey key)
        {
            return Raylib.IsKeyDown(key);
        }

        public static bool IsKeyTyped(KeyboardKey key)
        {
            return Raylib.IsKeyPressed(key);
        }

        public static bool IsKeyReleased(KeyboardKey key)
        {
            return Raylib.IsKeyReleased(key);
        }

        public static bool IsMouseButtonPressed(MouseButton button)
        {
            return Raylib.IsMouseButtonDown(button);
        }

        public static bool IsMouseButtonClicked(MouseButton button)
        {
            return Raylib.IsMouseButtonPressed(button);
        }

        public static bool IsMouseOnScreen()
        {
            return Raylib.IsCursorOnScreen();
        }

        public static Vector2 GetMousePosition()
        {
            return Raylib.GetMousePosition();
        }

        public static int GetMouseX()
        {
            return Raylib.GetMouseX();
        }

        public static int GetMouseY()
        {
            return Raylib.GetMouseY();
        }
    }
}
