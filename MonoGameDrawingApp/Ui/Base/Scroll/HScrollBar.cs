using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;

namespace MonoGameDrawingApp.Ui.Base.Scroll
{
    public class HScrollBar : ScrollBar
    {
        public HScrollBar(UiEnvironment environment) : base(environment)
        {
            InnerBar = new ChangeableView(environment, Bar);
            Inner = new HSplitStandard(environment, InnerBar, SecondBackground, 1);
            Outer = new HSplitStandard(environment, FirstBackground, Inner, 1);
        }

        protected override Rectangle GetBarBounds(Vector2 position, int width, int height, int dist, int length)
        {
            return new Rectangle((position + new Vector2(dist, 0)).ToPoint(), new Point(length, height));
            //return new Rectangle(0, 0, 0, 0);
        }

        protected override int GetMouseOffset(Vector2 position, int width, int height)
        {
            return (Mouse.GetState().Position - position.ToPoint()).X * End / width;
        }
    }
}
