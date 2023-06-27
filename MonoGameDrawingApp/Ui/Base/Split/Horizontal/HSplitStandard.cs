using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base.Split.Horizontal
{
    public class HSplitStandard : HSplit
    {
        private readonly SplitStandardHelper _splitHelper;
        public HSplitStandard(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition) : base(environment, first, second, splitPosition)
        {
            _splitHelper = new SplitStandardHelper();
        }

        private Vector2 SecondPosition => new(SplitPosition, 0);

        public override void Update(Vector2 position, int width, int height)
        {
            First.Update(position, SplitPosition, height);
            Second.Update(position + SecondPosition, width - SplitPosition, height);
            Changed = Changed || First.Changed || Second.Changed;
        }

        protected override Texture2D Render(Graphics graphics)
        {
            return _splitHelper.Render(graphics, Changed, Width, Height, SecondPosition, First, Second);
        }
    }
}
