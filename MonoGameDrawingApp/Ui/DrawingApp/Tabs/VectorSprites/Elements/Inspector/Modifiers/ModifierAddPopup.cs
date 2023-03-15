using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.TextInput;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Modifiers
{
    public class ModifierAddPopup : IUiElement
    {
        private const int Spacing = 10;
        private const int ButtonSpacing = 2;
        private const int InternalWidth = 200;
        private const int ListHeight = 500;

        private readonly List<ContextMenuButton> _buttons;
        private readonly VScrollableListView _visibleButtons;
        private ContextMenuButton _first;

        private readonly IUiElement _root;
        private readonly TextInputField _searchBar;

        public ModifierAddPopup(UiEnvironment environment, PopupEnvironment popupEnvironment, Action<IGeometryModifier> confirmed)
        {
            Environment = environment;

            _buttons = GeometryModifierRegistry.ModifierNames.Select(
                selector: (v) =>
                    new ContextMenuButton(Environment, v, () =>
                    {
                        confirmed(GeometryModifierRegistry.GenerateFromName(v));
                        popupEnvironment.Close();
                    }
                )
            ).ToList();

            _searchBar = new TextInputField(Environment, "", new ITextInputFilter[] { new AlphanumericTextInputFilter() });
            _searchBar.ValueChanged += _updateList;
            _searchBar.Deselected += popupEnvironment.Close;
            _searchBar.TextEntered += () =>
            {
                _first?.OnClick();
            };

            _searchBar.IsSelected = true;

            _visibleButtons = new VScrollableListView(Environment, _buttons, false, ButtonSpacing);

            _updateList();

            _root = new StackView(
                environment: Environment,
                children: new List<IUiElement>
                {
                    new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                    new VListView<IUiElement>(
                        environment: Environment,
                        items: new List<IUiElement>
                        {
                            new EmptySpace(Environment, 1, Spacing),
                            new HListView<IUiElement>(
                                environment: Environment,
                                items: new List<IUiElement>
                                {
                                    new EmptySpace(Environment, Spacing, 1),
                                    new MinSize(Environment, _searchBar, InternalWidth, 1),
                                    new EmptySpace(Environment, Spacing, 1),
                                }
                            ),
                            new EmptySpace(Environment, 1, Spacing),
                            new HListView<IUiElement>(
                                environment: Environment,
                                items: new List<IUiElement>
                                {
                                    new EmptySpace(Environment, Spacing, 1),
                                    new MinSize(
                                        environment: Environment,
                                        child: new ScrollWindow(
                                            environment: Environment,
                                            child: _visibleButtons
                                        ),
                                        width: InternalWidth,
                                        height: ListHeight
                                    ),
                                    new EmptySpace(Environment, Spacing, 1),
                                }
                            ),
                            new EmptySpace(Environment, 1, Spacing),
                        }
                    )
                }
            );
        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; set; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }

        private void _updateList()
        {
            IEnumerable<ContextMenuButton> buttons;
            if (_searchBar.Value == string.Empty)
            {
                buttons = _buttons;
            }
            else
            {
                buttons = _buttons.Where(
                    predicate: (button) =>
                        button.Title.ToLower().Contains(_searchBar.Value.ToLower())
                );
            }
            _first = buttons.FirstOrDefault();
            _visibleButtons.Items = buttons;
        }
    }
}
