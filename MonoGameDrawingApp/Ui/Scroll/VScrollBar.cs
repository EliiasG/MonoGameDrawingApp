using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Split.Vertical;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public class VScrollBar : ScrollBar
    {
        public VScrollBar(UiEnvironment environment) : base(environment)
        {
            _innerBar = new ChangeableView(environment, Bar);
            _inner = new VSplitStandard(environment, _innerBar, SecondBackground, 1);
            _outer = new VSplitStandard(environment, FirstBackground, _inner, 1);
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
