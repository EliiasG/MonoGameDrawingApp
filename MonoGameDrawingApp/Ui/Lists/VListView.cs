using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Lists
{
    public class VListView<T> : ListView<T> where T : IUiElement
    {
        private readonly RenderHelper _renderHelper;

        public VListView(List<T> items) : base(items)
        {
            _renderHelper = new RenderHelper();
        }

        public override int RequiredWidth => Items.Count > 0 ? Items.Max((T item) => item.RequiredWidth) : 3;

        public override int RequiredHeight => Items.Count > 0 ? Items.Sum((T item) => item.RequiredHeight) : 1;

        public override Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_changed || _renderHelper.SizeChanged)
            {
                _renderHelper.BeginDraw();

                List<Texture2D> renders = new List<Texture2D>();

                foreach (IUiElement item in Items.ToArray()) //using .ToArray(), since List may be modified while running loop
                {
                    renders.Add(item.Render(graphics, width, item.RequiredHeight));
                }

                Vector2 renderPosition = Vector2.Zero;
                foreach (Texture2D render in renders)
                {
                    graphics.SpriteBatch.Draw(
                        texture: render,
                        position: renderPosition,
                        color: Color.White
                    );
                    renderPosition += new Vector2(0, render.Height);
                }

                _renderHelper.FinishDraw();
            }

            _changed = false;
            return _renderHelper.Result;
        }

        protected override void _updateItems(Vector2 position, int width, int height)
        {
            Vector2 updatePositon = position;
            foreach (IUiElement item in Items)
            {
                item.Update(updatePositon, width, item.RequiredHeight);
                if (item.Changed)
                {
                    _changed = true;
                }
                updatePositon += new Vector2(0, item.RequiredHeight);
            }
        }
    }
}
