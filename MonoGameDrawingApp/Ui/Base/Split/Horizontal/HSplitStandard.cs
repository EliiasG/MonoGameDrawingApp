using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base.Split;

namespace MonoGameDrawingApp.Ui.Base.Split.Horizontal
{
    public class HSplitStandard : HSplit
    {
        private SplitStandardHelper _splitHelper;
        public HSplitStandard(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition) : base(environment, first, second, splitPosition)
        {
            _splitHelper = new SplitStandardHelper();
        }

        private Vector2 _secondPosition => new(SplitPosition, 0);

        public override void Update(Vector2 position, int width, int height)
        {
            First.Update(position, SplitPosition, height);
            Second.Update(position + _secondPosition, width - SplitPosition, height);
            _changed = _changed || First.Changed || Second.Changed;
        }

        protected override Texture2D _render(Graphics graphics)
        {
            return _splitHelper.Render(graphics, Changed, _width, _height, _secondPosition, First, Second);
        }
    }
}
