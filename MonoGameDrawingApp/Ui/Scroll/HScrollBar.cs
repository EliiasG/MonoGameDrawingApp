using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Split.Horizontal;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public class HScrollBar : ScrollBar
    {
        public HScrollBar(UiEnvironment environment) : base(environment)
        {
            _innerBar = new ChangeableView(environment, _bar);
            _inner = new HSplitStandard(environment, _innerBar, _secondBackground, 1);
            _outer = new HSplitStandard(environment, _firstBackground, _inner, 1);
        }

        protected override Rectangle _getBarBounds(Vector2 position, int width, int height, int dist, int length)
        {
            return new Rectangle((position + new Vector2(dist, 0)).ToPoint(), new Point(length, height));
            //return new Rectangle(0, 0, 0, 0);
        }

        protected override int _getMouseOffset(Vector2 position, int width, int height)
        {
            return (Mouse.GetState().Position - position.ToPoint()).X * End / width;
        }
    }
}
