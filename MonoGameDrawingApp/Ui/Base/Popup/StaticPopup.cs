using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base.Lists;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Popup
{
    public class StaticPopup : IUiElement
    {
        private const int Spacing = 8;

        private IUiElement _root;

        public StaticPopup(UiEnvironment environment, string message)
        {
            Environment = environment;
            _root = new StackView(Environment, new IUiElement[]
            {
                new ColorRect(Environment, Environment.Theme.SecondaryMenuBackgroundColor),
                new VListView<IUiElement>(Environment, new List<IUiElement>()
                {
                    new MinSize(Environment, new ColorRect(Environment, Color.Transparent), 1, Spacing),
                    new HListView<IUiElement>(Environment, new List<IUiElement>()
                    {
                        new MinSize(Environment, new ColorRect(Environment, Color.Transparent), Spacing, 1),
                        new ColorModifier(Environment, new TextView(Environment, message), Environment.Theme.DefaultTextColor),
                        new MinSize(Environment, new ColorRect(Environment, Color.Transparent), Spacing, 1),
                    }),
                    new MinSize(Environment, new ColorRect(Environment, Color.Transparent), 1, Spacing),
                }),
            });
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
