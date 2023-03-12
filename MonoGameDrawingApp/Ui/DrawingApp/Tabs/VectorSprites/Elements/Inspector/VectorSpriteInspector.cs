using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Modifiers;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector
{
    public class VectorSpriteInspector : IUiElement
    {
        private const int Spacing = 5;

        private readonly IUiElement _root;
        private readonly IUiElement _inspector;
        private readonly IUiElement _deselected;

        private readonly ChangeableView _changeableView;

        private readonly VectorSpriteTabView _vectorSpriteTabView;
        private readonly StringInspectorProperty _nameField;
        private readonly ColorInspectorProperty _colorField;

        public VectorSpriteInspector(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            _deselected = new ColorRect(Environment, Color.Transparent);
            _vectorSpriteTabView = vectorSpriteTabView;

            IUiElement title = new ColorModifier(Environment, new TextView(environment, "Inspector"), Environment.Theme.DefaultTextColor);

            _nameField = new StringInspectorProperty(Environment, "Name:", "", new ITextInputFilter[] { new AlphanumericTextInputFilter() }, () =>
            {
                if (_vectorSpriteTabView.Tree.Selected != null)
                {
                    vectorSpriteTabView.Selected.Name = _nameField.Value;
                }
            });

            _colorField = new ColorInspectorProperty(Environment, _vectorSpriteTabView.PopupEnvironment, "Color:", System.Drawing.Color.White, () =>
            {
                if (_vectorSpriteTabView.Tree.Selected != null)
                {
                    vectorSpriteTabView.Selected.Geometry.Color = _colorField.Value;
                }
            });


            _inspector = new VSplitStandard(
                environment: Environment,
                first: new VListView<IUiElement>(
                    environment: Environment,
                    items: new List<IUiElement>()
                    {
                        new EmptySpace(Environment, 1, Spacing),
                        _nameField,
                        new EmptySpace(Environment, 1, Spacing),
                        _colorField,
                        new EmptySpace(Environment, 1, Spacing),
                    }
                ),
                second: new GeometryModifierListInspectorView(Environment, vectorSpriteTabView),
                splitPosition: -1
            );

            _changeableView = new ChangeableView(Environment, _inspector);

            _root = new VSplitStandard(Environment, new CenterView(Environment, title, true, false), new StackView(Environment, new IUiElement[]
            {
                new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                new HSplitStandard(Environment, new EmptySpace(Environment,  5, 1), _changeableView, -1),
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
            _changeableView.Child = _vectorSpriteTabView.Selected == null ? _deselected : _inspector;

            _nameField.Value = _vectorSpriteTabView.Selected?.Name ?? "";
            _colorField.Value = _vectorSpriteTabView.Selected?.Geometry.Color ?? System.Drawing.Color.White;


            _root.Update(position, width, height);
        }
    }
}
