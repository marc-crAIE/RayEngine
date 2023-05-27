using RayEngine.Debug;
using RayEngine.GameObjects.Components;
using RayEngine.Utils;
using Raylib_cs;
using SharpMaths;

namespace RayEngine.Graphics
{
    public static class Renderer2D
    {
        public static void DrawQuad(Vector2 position, Vector2 scale, Colour colour, Texture2D? texture = null)
        {
            Matrix4 transform = Matrix4.Translation(position) * Matrix4.Scale(scale);
            DrawQuad(transform, colour, texture);
        }

        public static void DrawRotatedQuad(Vector2 position, Vector2 scale, float rotation, Colour color, Texture2D? texture = null)
        {
            Matrix4 transform = Matrix4.Translation(position) * Matrix4.RotationZ(rotation) * Matrix4.Scale(scale);
            DrawQuad(transform, color, texture);
        }

        public static void DrawQuad(Matrix4 transform, Colour colour, Texture2D? texture = null)
        {
            using var _it = Profiler.Function();

            (var translation, var rotation, var scale) = RMath.GetTransformRotationScale(transform);

            if (texture is not null)
            {

                Rectangle src = new Rectangle(0, 0, texture.GetWidth(), texture.GetHeight());
                Rectangle dst = new Rectangle(translation.x, translation.y, scale.x, scale.y);
                Raylib.DrawTexturePro(texture, src, dst, new Vector2(scale.x / 2.0f, scale.y / 2.0f), rotation.z, colour.ToColor());
            }
            else
            {
                (var t, var r, var s) = RMath.GetTransformRotationScale(transform);
                translation = t;
                rotation = SharpMath.ToDegrees(rotation);
                scale = s;

                Rectangle rect = new Rectangle(translation.x, translation.y, scale.x, scale.y);
                Raylib.DrawRectanglePro(rect, new Vector2(scale.x / 2.0f, scale.y / 2.0f), rotation.z, colour.ToColor());
            }
        }

        public static void DrawSprite(Matrix4 transform, SpriteComponent sprite)
        {
            DrawQuad(transform, sprite.Colour, sprite.Texture);
        }
    }
}
