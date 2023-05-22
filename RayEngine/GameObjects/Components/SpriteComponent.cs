using RayEngine.Graphics;
using SharpMaths;

namespace RayEngine.GameObjects.Components
{
    public class SpriteComponent
    {
        public Colour Colour = new Vector4(1.0f);
        public Texture2D? Texture = null;

        public SpriteComponent(Colour colour)
        {
            Colour = colour;
        }

        public SpriteComponent(Texture2D texture, Colour colour)
        {
            Texture = texture;
            Colour = colour;
        }

        public SpriteComponent(Texture2D texture)
        {
            Texture = texture;
            Colour = new Vector4(1.0f);
        }
    }
}
