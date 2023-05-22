using RayEngine.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Graphics
{
    public class Texture2D : Texture
    {
        internal Raylib_cs.Texture2D Texture { get; init; }

        public Texture2D(string filepath) : base(filepath)
        {
            using var _it = Profiler.Function();
            Texture = Raylib_cs.Raylib.LoadTexture(filepath);
        }

        ~Texture2D()
        {
            Dispose();
        }

        public override int GetWidth() => Texture.width;
        public override int GetHeight() => Texture.height;

        public override void Dispose()
        {
            using var _it = Profiler.Function();

            Raylib_cs.Raylib.UnloadTexture(Texture);
        }
    }
}
