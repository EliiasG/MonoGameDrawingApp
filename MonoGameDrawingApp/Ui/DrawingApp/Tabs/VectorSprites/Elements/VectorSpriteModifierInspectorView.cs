using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Descriptions;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteModifierInspectorView : IUiElement
    {
        const int IndentaionAmount = 8;

        private readonly IUiElement _root;

        public VectorSpriteModifierInspectorView(UiEnvironment environment, IVectorSpriteItemModifier modifier, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            VectorSpriteTabView = vectorSpriteTabView;
            Modifier = modifier;
            IModifierViewDescription description = _generateDescription(modifier);

            List<IUiElement> items = new()
            {
                new StackView(
                    environment: Environment,
                    children: new List<IUiElement>
                    {
                        new ColorRect(Environment, Environment.Theme.ButtonColor),
                        new ColorModifier(
                            environment: Environment,
                            child: new TextView(Environment, description.Name),
                            color: Environment.Theme.DefaultTextColor
                        ),
                    }
                )
            };

            foreach (IModifierViewProperty property in description.Properties)
            {
                items.Add(new StackView(
                    environment: Environment,
                    children: new List<IUiElement>
                    {
                        new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                        new HSplitStandard(
                            environment: Environment,
                            first: new EmptySpace(Environment, IndentaionAmount, 1),
                            second: property.Element,
                            splitPosition: -1
                        ),
                    }
                ));
            }

            _root = new VListView<IUiElement>(Environment, items);
        }

        public VectorSpriteTabView VectorSpriteTabView { get; init; }

        public IVectorSpriteItemModifier Modifier { get; init; }

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

        private static IModifierViewDescription _generateDescription(IVectorSpriteItemModifier modifier)
        {
            throw new NotImplementedException("No inspector implementation for: '" + modifier.GetType().Name + "'");
        }
    }
}
