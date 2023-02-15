using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base
{
    public class StackView : IUiElement
    {
        public readonly IEnumerable<IUiElement> Children;

        private readonly UiEnvironment _environment;

        private readonly RenderHelper _renderHelper;

        private List<IUiElement> _old;

        private bool _changed;

        public StackView(UiEnvironment environment, IEnumerable<IUiElement> children)
        {
            _environment = environment;
            Children = children;
            _renderHelper = new RenderHelper();
            _old = new List<IUiElement>();
        }

        public bool Changed => _changed;

        public int RequiredWidth => Children.Max((x) => x.RequiredWidth);

        public int RequiredHeight => Children.Max((x) => x.RequiredHeight);

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_renderHelper.SizeChanged || _changed)
            {
                List<Texture2D> renders = new();
                foreach (IUiElement child in Children)
                {
                    renders.Add(child.Render(graphics, width, height));
                }

                _renderHelper.BeginDraw();

                foreach (Texture2D render in renders)
                {
                    graphics.SpriteBatch.Draw(
                        texture: render,
                        position: Vector2.Zero,
                        color: Color.White
                    );
                }

                _renderHelper.FinishDraw();
            }

            _changed = false;
            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            foreach (IUiElement child in Children)
            {
                child.Update(position, width, height);
            }

            _updateChanged();
        }

        private void _updateChanged()
        {
            if (_changed) return;

            if (Children.Any((x) => x.Changed))
            {
                _changed = true;
                return;
            }

            if (Children.Count() != _old.Count())
            {
                _old = new List<IUiElement>(Children);
                _changed = true;
                return;
            }

            int i = 0;
            foreach (IUiElement child in Children) //must be foreach with index variable, because i cannot index in IEnumerator
            {
                if (child != _old[i])
                {
                    _changed = true;
                    _old = new List<IUiElement>(Children);
                    return;
                }
                ++i;
            }
        }
    }
}
