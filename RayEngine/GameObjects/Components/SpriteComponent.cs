using SharpMaths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.GameObjects.Components
{
    public class SpriteComponent
    {
        public Colour Colour = new Vector4(1.0f);

        public SpriteComponent(Colour colour)
        {
            Colour = colour;
        }
    }
}
