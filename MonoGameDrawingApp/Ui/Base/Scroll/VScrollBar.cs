using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;

namespace MonoGameDrawingApp.Ui.Base.Scroll
{
    public class VScrollBar : ScrollBar
    {
        public VScrollBar(UiEnvironment environment) : base(environment)
        {
            InnerBar = new ChangeableView(environment, Bar);
            Inner = new VSplitStandard(environment, InnerBar, SecondBackground, 1);
            Outer = new VSplitStandard(environment, FirstBackground, Inner, 1);
        }

        protected override Rectangle GetBarBounds(Vector2 position, int width, int height, int dist, int length)
        {
            return new Rectangle((position + new Vector2(0, dist)).ToPoint(), new Point(width, length));
        }

        protected override int GetMouseOffset(Vector2 position, int width, int height)
        {
            return (Mouse.GetState().Position - position.ToPoint()).Y * End / height;
        }
    }
}
