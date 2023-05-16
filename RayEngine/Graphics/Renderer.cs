using RayEngine.Utils;
using Raylib_cs;
using SharpMaths;

namespace RayEngine.Graphics
{
    public static class Renderer
    {
        public static bool Rendering { get; private set; }
        public static Colour ClearColor { get; set; }

        public static void Begin()
        {
            if (Rendering)
                throw new InvalidOperationException("The renderer is already rendering!");

            Raylib.ClearBackground(new Colour(0, 0, 0, 0).ToColor());
            Raylib.BeginDrawing();
            Rendering = true;
        }

        public static void End()
        {
            if (!Rendering)
                throw new InvalidOperationException("The renderer is not currently rendering!");

            Raylib.EndDrawing();
            Rendering = false;
        }
    }
}
