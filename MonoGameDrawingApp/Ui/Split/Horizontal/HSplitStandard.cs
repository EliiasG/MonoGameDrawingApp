using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Split.Horizontal
{
    public class HSplitStandard : HSplit
    {
        private SplitStandardHelper _splitHelper;
        public HSplitStandard(IUiElement first, IUiElement second, int splitPosition) : base(first, second, splitPosition)
        {
            _splitHelper = new SplitStandardHelper();
        }

        protected override Texture2D _render(Graphics graphics, Vector2 position)
        {
            Vector2 secondPosition = new Vector2(SplitPosition, 0);

            return _splitHelper.Render(graphics, position, _width, _height, secondPosition, First, Second);
        }
    }
}
