using RayEngine.Utils;
using Raylib_cs;
using SharpMaths;

namespace RayEngine.Graphics
{
    public static class Renderer2D
    {
        public static void DrawQuad(Vector2 position, Vector2 scale, Colour colour)
        {
            Matrix4 transform = Matrix4.Translation(position) * Matrix4.Scale(scale);
            DrawQuad(transform, colour);
        }

        public static void DrawRotatedQuad(Vector2 position, Vector2 scale, float rotation, Colour color)
        {
            Matrix4 transform = Matrix4.Translation(position) * Matrix4.RotationZ(rotation) * Matrix4.Scale(scale);
            DrawQuad(transform, color);
        }

        public static void DrawQuad(Matrix4 transform, Colour colour)
        {
            Vector3 scale = new Vector3(
                ((Vector3)transform[0]).Magnitude(), 
                ((Vector3)transform[1]).Magnitude(), 
                ((Vector3)transform[2]).Magnitude());

            Matrix3 rotationMatrix = (Matrix3)transform;
            rotationMatrix[0] /= scale.x;
            rotationMatrix[1] /= scale.y;
            rotationMatrix[2] /= scale.z;
            float rotation = SharpMath.ToDegrees((float)Math.Atan2(rotationMatrix[0].y, rotationMatrix[0].x));

            Vector2 translation = transform[3];

            Rectangle rect = new Rectangle(translation.x, translation.y, scale.x, scale.y);
            Raylib.DrawRectanglePro(rect, new Vector2(scale.x / 2.0f, scale.y / 2.0f), rotation, colour.ToColor());
        }
    }
}
