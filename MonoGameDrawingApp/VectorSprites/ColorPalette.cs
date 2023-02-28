using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites
{
    public class ColorPalette
    {
        public ColorPalette(IEnumerable<Color> colors, string name)
        {
            Colors = colors.ToArray();
            Name = name;
        }

        public Color[] Colors { get; init; }

        public string Name { get; init; }
    }
}
