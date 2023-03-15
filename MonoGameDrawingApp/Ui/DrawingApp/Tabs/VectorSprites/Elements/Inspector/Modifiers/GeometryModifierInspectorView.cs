using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties.Typed;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Modifiers
{
    public class GeometryModifierInspectorView : IUiElement
    {
        const int IndentaionAmount = 8;

        private readonly IUiElement _root;

        private readonly IEnumerable<IModifierParameterView> parameterViews;

        public GeometryModifierInspectorView(UiEnvironment environment, IGeometryModifier modifier, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            VectorSpriteTabView = vectorSpriteTabView;
            Modifier = modifier;

            List<IUiElement> items = new()
            {
                new StackView(
                    environment: Environment,
                    children: new List<IUiElement>
                    {
                        new ColorRect(Environment, Environment.Theme.ButtonColor),
                        new ColorModifier(
                            environment: Environment,
                            child: new TextView(Environment, modifier.Name),
                            color: Environment.Theme.DefaultTextColor
                        ),
                    }
                )
            };

            parameterViews = modifier.Parameters.Select(
                (IGeometryModifierParameter parameter) => _generateParameterView(parameter)
            );

            foreach (IModifierParameterView property in parameterViews)
            {
                items.Add(new StackView(
                    environment: Environment,
                    children: new List<IUiElement>
                    {
                        new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                        new HSplitStandard(
                            environment: Environment,
                            first: new EmptySpace(Environment, IndentaionAmount, 1),
                            second: property.GenerateElement(environment),
                            splitPosition: -1
                        ),
                    }
                ));
            }

            _root = new SmartButton(
                environment: Environment,
                child: new VListView<IUiElement>(Environment, items),
                rightClicked: () =>
                {
                    GeometryModifierContextMenu menu = new(Environment, vectorSpriteTabView.PopupEnvironment, vectorSpriteTabView.Selected, modifier);
                    vectorSpriteTabView.PopupEnvironment.Open(Mouse.GetState().Position, menu);
                }
            );
        }

        public void Done()
        {
            foreach (IModifierParameterView parameterView in parameterViews)
            {
                parameterView.Done();
            }
        }

        public VectorSpriteTabView VectorSpriteTabView { get; init; }

        public IGeometryModifier Modifier { get; init; }

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

        private IModifierParameterView _generateParameterView(IGeometryModifierParameter parameter)
        {
            return parameter switch
            {
                GeometryModifierParameter<int> p => new IntModifierParameterView(p),
                GeometryModifierParameter<float> p => new FloatModifierParameterView(p),
                GeometryModifierParameter<System.Numerics.Vector2> p => new Vector2ModifierParameterView(p),
                GeometryModifierParameter<bool> p => new BoolModifierParameterView(p),
                GeometryModifierParameter<System.Drawing.Color> p => new ColorModifierParameterView(p, VectorSpriteTabView.PopupEnvironment),
                _ => throw new NotImplementedException("Cannot display '" + parameter.GetType().Name + "'")
            };
        }
    }
}
