using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base
{
    public class StackView : IUiElement
    {
        private readonly RenderHelper _renderHelper;

        private List<IUiElement> _old;

        public StackView(UiEnvironment environment, IEnumerable<IUiElement> children)
        {
            Environment = environment;
            Children = children;
            _renderHelper = new RenderHelper();
            _old = new List<IUiElement>();
        }

        public bool Changed { get; private set; }

        public int RequiredWidth => Children.Max((x) => x.RequiredWidth);

        public int RequiredHeight => Children.Max((x) => x.RequiredHeight);

        public UiEnvironment Environment { get; }

        public IEnumerable<IUiElement> Children { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_renderHelper.SizeChanged || Changed)
            {
                List<Texture2D> renders = new();
                foreach (IUiElement child in Children)
                {
                    renders.Add(child.Render(graphics, width, height));
                }

                _renderHelper.BeginSpriteBatchDraw();

                foreach (Texture2D render in renders)
                {
                    graphics.SpriteBatch.Draw(
                        texture: render,
                        position: Vector2.Zero,
                        color: Color.White
                    );
                }

                _renderHelper.FinishSpriteBatchDraw();
            }

            Changed = false;
            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            foreach (IUiElement child in Children)
            {
                child.Update(position, width, height);
            }

            UpdateChanged();
        }

        private void UpdateChanged()
        {
            if (Changed)
            {
                return;
            }

            if (Children.Any((x) => x.Changed))
            {
                Changed = true;
                return;
            }

            if (Children.Count() != _old.Count)
            {
                _old = new List<IUiElement>(Children);
                Changed = true;
                return;
            }

            int i = 0;
            foreach (IUiElement child in Children) //must be foreach with index variable, because i cannot index in IEnumerator
            {
                if (child != _old[i])
                {
                    Changed = true;
                    _old = new List<IUiElement>(Children);
                    return;
                }
                ++i;
            }
        }
    }
}
