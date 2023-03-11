using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Modifiers.Applyable;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Modifiers
{
    public class VectorSpriteModifierContextMenu : IUiElement
    {
        private readonly IUiElement _root;

        public VectorSpriteModifierContextMenu(UiEnvironment environment, PopupEnvironment popupEnvironment, VectorSpriteItem item, IVectorSpriteItemModifier modifier)
        {
            Environment = environment;

            _root = new ContextMenu(
                environment: Environment,
                items: new IUiElement[]
                {
                    new ContextMenuButton(Environment, "Remove", () =>
                        {
                            item.RemoveModifier(modifier);
                            popupEnvironment.Close();
                        }
                    ),
                    new ContextMenuButton(Environment, "Move Up", () =>
                        {
                            item.MoveModifierUp(modifier);
                            popupEnvironment.Close();
                        }
                    )
                    {
                        Disabled = item.Modifiers.First() == modifier,
                    },
                    new ContextMenuButton(Environment, "Move Down", () =>
                        {
                            item.MoveModifierDown(modifier);
                            popupEnvironment.Close();
                        }
                    )
                    {
                        Disabled= item.Modifiers.Last() == modifier,
                    },
                    new ContextMenuButton(Environment, "Apply", () =>
                        {
                            if (modifier is IApplyableVectorSpriteItemModifier applyable)
                            {
                                applyable.Apply(item);
                                item.RemoveModifier(modifier);
                            }
                            popupEnvironment.Close();
                        }
                    )
                    {
                        Disabled = item.Modifiers.First() != modifier || modifier is not IApplyableVectorSpriteItemModifier
                    },
                },
                popupEnvironment: popupEnvironment
            );
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
