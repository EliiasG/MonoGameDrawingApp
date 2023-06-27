using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base.Split.Vertical
{
    internal class VSplitStandard : VSplit
    {
        private readonly SplitStandardHelper _splitHelper;
        public VSplitStandard(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition) : base(environment, first, second, splitPosition)
        {
            _splitHelper = new SplitStandardHelper();
        }
        private Vector2 SecondPosition => new(0, SplitPosition);

        public override void Update(Vector2 position, int width, int height)
        {
            First.Update(position, width, SplitPosition);
            Second.Update(position + SecondPosition, width, height - SplitPosition);
            Changed = Changed || First.Changed || Second.Changed;
        }

        protected override Texture2D Render(Graphics graphics)
        {
            return _splitHelper.Render(graphics, Changed, Width, Height, SecondPosition, First, Second);
        }
    }
}
