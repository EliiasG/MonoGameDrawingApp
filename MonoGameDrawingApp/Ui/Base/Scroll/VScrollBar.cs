using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;

namespace MonoGameDrawingApp.Ui.Base.Scroll
{
    public class VScrollBar : ScrollBar
    {
        public VScrollBar(UiEnvironment environment) : base(environment)
        {
            _innerBar = new ChangeableView(environment, _bar);
            _inner = new VSplitStandard(environment, _innerBar, _secondBackground, 1);
            _outer = new VSplitStandard(environment, _firstBackground, _inner, 1);
        }

        protected override Rectangle _getBarBounds(Vector2 position, int width, int height, int dist, int length)
        {
            return new Rectangle((position + new Vector2(0, dist)).ToPoint(), new Point(width, length));
        }

        protected override int _getMouseOffset(Vector2 position, int width, int height)
        {
            return (Mouse.GetState().Position - position.ToPoint()).Y * End / height;
        }
    }
}
