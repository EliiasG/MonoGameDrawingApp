using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Split
{
    internal class VSplitStandard : VSplit
    {
        public VSplitStandard(IUiElement first, IUiElement second, int splitPosition) : base(first, second, splitPosition) {}

        protected override Texture2D _render(Graphics graphics, Vector2 position)
        {
            Vector2 secondPosition = new Vector2(0, SplitPosition);

            Texture2D firstRender = First.Render(graphics, position, _width, SplitPosition);
            Texture2D secondRender = Second.Render(graphics, position + secondPosition, _width, _height - SplitPosition);

            ElementBuilder elementBuilder = new ElementBuilder(graphics, _width, _height);

            graphics.SpriteBatch.Draw(
                texture: firstRender,
                position: new Vector2(0),
                color: Color.White
            );

            graphics.SpriteBatch.Draw(
                texture: secondRender,
                position: secondPosition,
                color: Color.White
            );

            return elementBuilder.Finish();
        }
    }
}
