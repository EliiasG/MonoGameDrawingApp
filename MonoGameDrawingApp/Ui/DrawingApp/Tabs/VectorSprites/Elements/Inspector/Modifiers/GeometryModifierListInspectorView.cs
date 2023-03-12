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
using MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Modifiers
{
    public class GeometryModifierListInspectorView : IUiElement
    {
        private readonly IUiElement _root;
        private readonly VScrollableListView _modifierElementsView;

        private VectorSpriteItem _oldSelected;
        private IGeometryModifier[] _modifiers;
        private GeometryModifierInspectorView[] _modifierElements;

        public GeometryModifierListInspectorView(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            VectorSpriteTabView = vectorSpriteTabView;

            _modifierElementsView = new VScrollableListView(
                environment: Environment,
                items: Array.Empty<IUiElement>(),
                updateOutOfView: true,
                spacing: 2
            );

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
                        vectorSpriteTabView.PopupEnvironment.Open(Mouse.GetState().Position, new ModifierAddPopup(Environment, vectorSpriteTabView.PopupEnvironment, (modifier) =>
                        {
                            vectorSpriteTabView.Selected.AddModifier(modifier);
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
                            child: _modifierElementsView
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

        public void Reload()
        {
            if (VectorSpriteTabView.Selected == null)
            {
                _modifierElementsView.Items = Array.Empty<IUiElement>();
                return;
            }
            IEnumerable<IGeometryModifier> itemModifiers = VectorSpriteTabView.Selected.Modifiers;

            if (_modifiers?.Length != itemModifiers.Count())
            {
                _modifiers = new IGeometryModifier[itemModifiers.Count()];
                foreach (GeometryModifierInspectorView _modifierElement in _modifierElements ?? Array.Empty<GeometryModifierInspectorView>())
                {
                    _modifierElement.Done();
                }
                _modifierElements = new GeometryModifierInspectorView[itemModifiers.Count()];
            }

            int i = 0;
            foreach (IGeometryModifier modifier in itemModifiers)
            {
                if (modifier != _modifiers[i])
                {
                    _modifiers[i] = modifier;
                    _modifierElements[i]?.Done();
                    _modifierElements[i] = new GeometryModifierInspectorView(Environment, modifier, VectorSpriteTabView);
                }
                ++i;
            }
            _modifierElementsView.Items = _modifierElements;
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
                if (_oldSelected != null)
                {
                    _oldSelected.GetAttachment<ChangeListenerVectorSpriteItemAttachment>().Changed -= Reload;
                }
                Reload();
                VectorSpriteTabView.Selected.GetAttachment<ChangeListenerVectorSpriteItemAttachment>().Changed += Reload;
                _oldSelected = VectorSpriteTabView.Selected;
            }
        }
    }
}
