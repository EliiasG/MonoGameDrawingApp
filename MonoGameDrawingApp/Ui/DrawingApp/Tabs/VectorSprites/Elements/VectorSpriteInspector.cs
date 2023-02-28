using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Properties;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree;
using MonoGameDrawingApp.VectorSprites;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
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
                    _selected.Name = _nameField.Value;
                }
            });

            _colorField = new ColorInspectorProperty(Environment, vectorSpriteTabView.PopupEnvironment, "Color:", System.Drawing.Color.White, () =>
            {
                if (_vectorSpriteTabView.Tree.Selected != null)
                {
                    _selected.Geometry.Color = _colorField.Value;
                }
            });


            _inspector = new VScrollableListView(
                environment: Environment,
                items: new List<IUiElement>()
                {
                    new EmptySpace(Environment,  1, Spacing),
                    _nameField,
                    _colorField,
                },
                updateOutOfView: false,
                spacing: Spacing
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

        private VectorSpriteItem _selected => (_vectorSpriteTabView.Tree.Selected as VectorSpriteTreeItem)?.Item;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _changeableView.Child = _selected == null ? _deselected : _inspector;

            _nameField.Value = _selected?.Name ?? "";
            _colorField.Value = _selected?.Geometry.Color ?? System.Drawing.Color.White;


            _root.Update(position, width, height);
        }
    }
}
