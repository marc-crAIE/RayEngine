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
        private Raylib_cs.Texture2D Texture;

        public uint ID
        {
            get => Texture.id;
            set => Texture.id = value;
        }

        public int Width
        {
            get => Texture.width;
            set => Texture.width = value;
        }

        public int Height
        {
            get => Texture.height;
            set => Texture.height = value;
        }

        public Texture2D(string filepath) : base(filepath)
        {
            using var _it = Profiler.Function();
            Texture = Raylib_cs.Raylib.LoadTexture(filepath);
        }

        public Texture2D(Raylib_cs.Texture2D texture) : this(texture.GetType().ToString())
        {
            using var _it = Profiler.Function();
            Texture = texture;
        }

        ~Texture2D()
        {
            Dispose();
        }

        public override int GetWidth() => Texture.width;
        public override int GetHeight() => Texture.height;
        public override ref uint GetID() => ref Texture.id;

        public static implicit operator Raylib_cs.Texture2D(Texture2D texture) => texture.Texture;
        public static implicit operator Texture2D(Raylib_cs.Texture2D texture) => new Texture2D(texture);

        public override void Dispose()
        {
            using var _it = Profiler.Function();

            Raylib_cs.Raylib.UnloadTexture(Texture);
        }
    }
}
