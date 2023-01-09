using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Lists
{
    public class HListView<T> : ListView<T> where T : IUiElement
    {
        private readonly RenderHelper _renderHelper;

        public HListView(List<T> items) : base(items)
        {
            _renderHelper = new RenderHelper();
        }

        public override int RequiredWidth => Items.Count > 0 ? Items.Sum((T item) => item.RequiredWidth) : 1;

        public override int RequiredHeight => Items.Count > 0 ? Items.Max((T item) => item.RequiredHeight) : 3;

        public override Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);
 
            if (Changed || _renderHelper.SizeChanged)
            {
                List<Texture2D> renders = new List<Texture2D>();

                foreach (IUiElement item in Items)
                {
                    renders.Add(item.Render(graphics, item.RequiredWidth, height));
                }

                _renderHelper.BeginDraw();

                Vector2 renderPosition = Vector2.Zero;
                foreach (Texture2D render in renders)
                {
                    graphics.SpriteBatch.Draw(
                        texture: render,
                        position: renderPosition,
                        color: Color.White
                    );
                    renderPosition += new Vector2(render.Width, 0);
                }

                _renderHelper.FinishDraw();
            }

            _changed = false;
            return _renderHelper.Result;
        }

        protected override void _updateItems(Vector2 position, int width, int height)
        {
            Vector2 updatePositon = position;
            foreach (IUiElement item in new List<T>(Items))
            {
                item.Update(updatePositon, item.RequiredWidth, height);
                if(item.Changed)
                {
                    _changed = true;
                }
                updatePositon += new Vector2(item.RequiredWidth, 0);
            }
        }
    }
}
