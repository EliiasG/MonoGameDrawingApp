using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;
using MonoGameDrawingApp.VectorSprites;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteModifierListInspectorView : IUiElement
    {
        private readonly IUiElement _root;

        private readonly List<IUiElement> _modifierList = new();

        public VectorSpriteModifierListInspectorView(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            VectorSpriteTabView = vectorSpriteTabView;

            _root = new VSplitStandard(
                environment: Environment,
                first: new ColorModifier(
                    environment: Environment,
                    child: new TextView(Environment, "Modifiers:"),
                    color: Environment.Theme.DefaultTextColor
                ),
                second: new StackView(
                    environment: Environment,
                    children: new List<IUiElement>()
                    {
                        new ColorRect(Environment, Environment.Theme.SecondaryMenuBackgroundColor)
                    }
                ),
                splitPosition: -1
            );
        }

        public VectorSpriteTabView VectorSpriteTabView { get; init; }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public void Reload(VectorSpriteItem item)
        {
            _modifierList.Clear();
            if (item == null)
            {
                return;
            }
            _modifierList.Capacity = item.Modifiers.Count();
            foreach (var modifier in item.Modifiers)
            {
                _modifierList.Add(new VectorSpriteModifierInspectorView(Environment, modifier, VectorSpriteTabView));
            }
        }

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
