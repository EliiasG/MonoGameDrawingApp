using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Serialization;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteItemContextMenu : IUiElement
    {
        private readonly IUiElement _root;

        public VectorSpriteItemContextMenu(UiEnvironment environment, VectorSpriteItem item, PopupEnvironment popupEnvironment)
        {
            Environment = environment;
            Item = item;
            PopupEnvironment = popupEnvironment;

            _root = new ContextMenu(Environment, new IUiElement[]
            {
                new ContextMenuButton(Environment, Item.IsVisible ? "Hide" : "Show", ChangeVisible),
                new ContextMenuSeperator(Environment),
                new ContextMenuButton(Environment, "Rename", Rename),
                new ContextMenuButton(Environment, "Delete", Delete)
                {
                    Disabled = item.Parent == null,
                },
                new ContextMenuSeperator(Environment),
                new ContextMenuButton(Environment, "Cut", Cut)
                {
                    Disabled = item.Parent == null,
                },
                new ContextMenuButton(Environment, "Copy", Copy),
                new ContextMenuButton(Environment, "Paste", Paste)
                {
                    Disabled = Environment.Clipboard is not SerializableVectorSpriteItem,
                },
                new ContextMenuSeperator(Environment),
                new ContextMenuButton(Environment, "Add Item", AddItem),
                new ContextMenuSeperator(Environment),
                new ContextMenuButton(Environment, "Move Up", MoveUp)
                {
                    Disabled = Item.Parent == null || Item.Parent.Children.First() == Item
                },
                new ContextMenuButton(Environment, "Move Down", MoveDown)
                {
                    Disabled = Item.Parent == null || Item.Parent.Children.Last() == Item
                },
            }, popupEnvironment);
        }

        public VectorSpriteItem Item { get; set; }

        public PopupEnvironment PopupEnvironment { get; init; }

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

        private void Rename()
        {
            PopupEnvironment.OpenCentered(new TextInputPopup(
                environment: Environment,
                popupEnvironment: PopupEnvironment,
                confirmed: (newName) =>
                {
                    Item.Name = newName;
                },
                filters: new ITextInputFilter[] { new AlphanumericTextInputFilter() },
                title: "Rename '" + Item.Name + "'",
                currentValue: Item.Name
            ));
        }

        private void Delete()
        {
            PopupEnvironment.OpenCentered(new ChoicePopup(Environment, "Delete '" + Item.Name + "'", PopupEnvironment, new ChoicePopupOption[]
            {
                new ChoicePopupOption("Cancel", () => {}),
                new ChoicePopupOption("Confirm", () => Item.Parent.RemoveChild(Item)),
            }));
        }

        private void Copy()
        {
            Environment.Clipboard = new SerializableVectorSpriteItem(Item);
            PopupEnvironment.Close();
        }

        private void Cut()
        {
            Copy();
            Item.Parent.RemoveChild(Item);
        }

        private void Paste()
        {
            if (Environment.Clipboard is SerializableVectorSpriteItem serializableItem)
            {
                Item.AddChild(serializableItem.ToItem(Item.Sprite));
            }
            PopupEnvironment.Close();
        }

        private void AddItem()
        {
            PopupEnvironment.OpenCentered(new TextInputPopup(
                environment: Environment,
                popupEnvironment: PopupEnvironment,
                confirmed: (name) =>
                {
                    Item.AddChild(new VectorSpriteItem(name, Item.Sprite));
                },
                filters: new ITextInputFilter[] { new AlphanumericTextInputFilter() },
                title: "Add item",
                currentValue: "NewItem"
            ));
        }

        private void MoveUp()
        {
            Item.MoveUp();
            PopupEnvironment.Close();
        }

        private void MoveDown()
        {
            Item.MoveDown();
            PopupEnvironment.Close();
        }

        private void ChangeVisible()
        {
            Item.IsVisible = !Item.IsVisible;
            PopupEnvironment.Close();
        }
    }
}
