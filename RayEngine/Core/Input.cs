using Raylib_cs;
using SharpMaths;

namespace RayEngine.Core
{
    public static class Input
    {
        public static bool IsKeyPressed(Key key)
        {
            return Raylib.IsKeyDown((Raylib_cs.KeyboardKey)key);
        }

        public static bool IsKeyTyped(KeyboardKey key)
        {
            return Raylib.IsKeyPressed((Raylib_cs.KeyboardKey)key);
        }

        public static bool IsKeyReleased(KeyboardKey key)
        {
            return Raylib.IsKeyReleased((Raylib_cs.KeyboardKey)key);
        }

        public static bool IsMouseButtonPressed(Mouse button)
        {
            return Raylib.IsMouseButtonDown((Raylib_cs.MouseButton)button);
        }

        public static bool IsMouseButtonClicked(Mouse button)
        {
            return Raylib.IsMouseButtonPressed((Raylib_cs.MouseButton)button);
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
