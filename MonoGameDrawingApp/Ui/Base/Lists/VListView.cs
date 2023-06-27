using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.Lists
{
    public class VListView<T> : ListView<T> where T : IUiElement
    {
        private readonly RenderHelper _renderHelper;

        public VListView(UiEnvironment environment, List<T> items) : base(environment, items)
        {
            _renderHelper = new RenderHelper();
        }

        public override int RequiredWidth => Items.Count > 0 ? Items.Max((item) => item.RequiredWidth) : 3;

        public override int RequiredHeight => Items.Count > 0 ? Items.Sum((item) => item.RequiredHeight) + ((Items.Count - 1) * Spacing) : 1;

        public override Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                List<Texture2D> renders = new();

                //using .ToArray(), since List may be modified while running loop
                foreach (IUiElement item in Items.ToArray())
                {
                    renders.Add(item.Render(graphics, width, item.RequiredHeight));
                }

                _renderHelper.BeginSpriteBatchDraw();

                Vector2 renderPosition = Vector2.Zero;
                foreach (Texture2D render in renders)
                {
                    graphics.SpriteBatch.Draw(
                        texture: render,
                        position: renderPosition,
                        color: Color.White
                    );
                    renderPosition += new Vector2(0, render.Height + Spacing);
                }

                _renderHelper.FinishSpriteBatchDraw();
            }

            Changed = false;
            return _renderHelper.Result;
        }

        protected override void UpdateItems(Vector2 position, int width, int height)
        {
            Vector2 updatePositon = position;
            foreach (IUiElement item in Items)
            {
                item.Update(updatePositon, width, item.RequiredHeight);
                if (item.Changed)
                {
                    Changed = true;
                }
                updatePositon += new Vector2(0, item.RequiredHeight + Spacing);
            }
        }
    }
}
