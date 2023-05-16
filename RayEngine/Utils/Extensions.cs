using Raylib_cs;
using SharpMaths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Converts SharpMaths Colour data type to Raylib's Color data type
        /// </summary>
        /// <param name="colour">SharpMaths Colour</param>
        /// <returns>Raylib Color</returns>
        public static Color ToColor(this Colour colour)
        {
            return new Color(colour.red, colour.green, colour.blue, colour.alpha);
        }

        /// <summary>
        /// Converts Raylib's Color data type to SharpMaths Colour data type
        /// </summary>
        /// <param name="colour">Raylib Color</param>
        /// <returns>SharpMaths Colour</returns>
        public static Colour ToColour(this Color colour)
        {
            return new Colour(colour.r, colour.g, colour.b, colour.a);
        }
    }
}
