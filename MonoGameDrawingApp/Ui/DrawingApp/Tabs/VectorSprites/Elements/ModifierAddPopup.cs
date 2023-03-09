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
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Modifiers.Applyable.Simple;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class ModifierAddPopup : IUiElement
    {
        private const int Spacing = 10;
        private const int ButtonSpacing = 2;
        private const int InternalWidth = 200;
        private const int ListHeight = 500;

        private static readonly (string, Func<IVectorSpriteItemModifier>)[] s_modifiers =
        {
            ("Move", () => new MoveModifier()),
            ("Foo", () => new MoveModifier()),
            ("Bar", () => new MoveModifier()),
            ("Baz", () => new MoveModifier()),
            ("FooBar", () => new MoveModifier()),
            ("ABC", () => new MoveModifier()),
            ("BCD", () => new MoveModifier()),
            ("CDE", () => new MoveModifier()),
        };

        private readonly List<ContextMenuButton> _buttons;
        private VScrollableListView _visibleButtons;

        private readonly IUiElement _root;
        private readonly TextInputField _searchBar;

        public ModifierAddPopup(UiEnvironment environment, PopupEnvironment popupEnvironment, VectorSpriteItem item)
        {
            Environment = environment;

            _buttons = s_modifiers.Select(
                selector: ((string, Func<IVectorSpriteItemModifier>) v) =>
                    new ContextMenuButton(Environment, v.Item1, () =>
                    {
                        item.AddModifier(v.Item2());
                        popupEnvironment.Close();
                    }
                )
            ).ToList();

            _searchBar = new TextInputField(Environment, "", new ITextInputFilter[] { new AlphanumericTextInputFilter() });
            _searchBar.ValueChanged += _updateList;
            //TODO make the menu close when deselecting the text, and confirm on the top element when text is entered

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
            if (_searchBar.Value == string.Empty)
            {
                _visibleButtons.Items = _buttons;
            }
            else
            {
                _visibleButtons.Items = _buttons.Where(
                    predicate: (ContextMenuButton button) =>
                        button.Title.ToLower().Contains(_searchBar.Value.ToLower())
                );
            }
        }
    }
}
