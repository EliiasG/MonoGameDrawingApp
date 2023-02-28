using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Properties
{
    public class NamedInspectorProperty : IUiElement
    {
        private readonly IUiElement _root;

        public NamedInspectorProperty(UiEnvironment environment, IUiElement child, string text)
        {
            Environment = environment;
            Child = child;
            Text = text;

            _root = new HSplitStandard(
                environment: Environment,
                first: new CenterView(
                    environment: Environment,
                    child: new ColorModifier(
                        environment: Environment,
                        child: new TextView(Environment, Text),
                        color: Environment.Theme.DefaultTextColor
                    ),
                    centerHorizontal: false,
                    centerVertical: true
                ),
                second: Child,
                splitPosition: -2
            );
        }

        public string Text { get; init; }

        public IUiElement Child { get; init; }

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
