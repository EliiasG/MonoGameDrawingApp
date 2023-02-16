using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteInspector : IUiElement
    {
        private readonly IUiElement _root;
        private readonly IUiElement _inspector;
        private readonly IUiElement _deselected;

        private readonly ChangeableView _changeableView;

        private readonly TextView _title;
        private UiEnvironment environment;
        private VectorSpriteTabView vectorSpriteTabView;

        public VectorSpriteInspector(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            _deselected = new ColorRect(Environment, Color.Transparent);
            _changeableView = new ChangeableView(Environment, _deselected);

            _title = new TextView(environment, "Inspector");

            _root = new VSplitStandard(Environment, new CenterView(Environment, new ColorModifier(Environment, _title, Environment.Theme.DefaultTextColor), true, false), new StackView(Environment, new IUiElement[]
            {
                new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                _changeableView,
            }), -1);
        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }
    }
}
