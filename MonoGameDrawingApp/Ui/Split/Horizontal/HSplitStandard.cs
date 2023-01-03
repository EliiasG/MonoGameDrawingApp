using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Split.Horizontal
{
    public class HSplitStandard : HSplit
    {
        private SplitStandardHelper _splitHelper;
        public HSplitStandard(IUiElement first, IUiElement second, int splitPosition) : base(first, second, splitPosition)
        {
            _splitHelper = new SplitStandardHelper();
        }

        private Vector2 _secondPosition => new Vector2(SplitPosition, 0);

        public override void Update(Vector2 position, int width, int height)
        {
            First.Update(position, width, height);
            Second.Update(position + _secondPosition, width, height);
            _changed = _changed || First.Changed || Second.Changed;
        }

        protected override Texture2D _render(Graphics graphics)
        {
            return _splitHelper.Render(graphics, Changed, _width, _height, _secondPosition, First, Second);
        }
    }
}
