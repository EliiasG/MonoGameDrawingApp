using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteModifierListInspectorView : IUiElement
    {
        private readonly IUiElement _root;

        private VectorSpriteItem _oldSelected;

        private readonly List<IUiElement> _modifierList = new();

        public VectorSpriteModifierListInspectorView(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            VectorSpriteTabView = vectorSpriteTabView;

            _root = new VSplitStandard(
                environment: Environment,
                first: new HSplitStandard(
                    environment: Environment,
                    first: new ColorModifier(
                        environment: Environment,
                        child: new TextView(Environment, "Modifiers:"),
                        color: Environment.Theme.DefaultTextColor
                    ),
                    second: new ContextMenuButton(Environment, "+  ", () =>
                    {
                        vectorSpriteTabView.PopupEnvironment.Open(Mouse.GetState().Position, new ModifierAddPopup(Environment, vectorSpriteTabView.PopupEnvironment, (IVectorSpriteItemModifier modifier) =>
                        {
                            vectorSpriteTabView.Selected.AddModifier(modifier);
                            Reload(vectorSpriteTabView.Selected);
                        }));
                    }),
                    splitPosition: -2
                ),
                second: new StackView(
                    environment: Environment,
                    children: new List<IUiElement>()
                    {
                        new ColorRect(Environment, Environment.Theme.SecondaryMenuBackgroundColor),
                        new ScrollWindow(
                            environment: Environment,
                            child: new VScrollableListView(
                                environment: Environment,
                                items: _modifierList,
                                updateOutOfView: true,
                                spacing: 2
                            )
                        ),
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
            if (_oldSelected != VectorSpriteTabView.Selected)
            {
                Reload(VectorSpriteTabView.Selected);
                _oldSelected = VectorSpriteTabView.Selected;
            }
        }
    }
}
