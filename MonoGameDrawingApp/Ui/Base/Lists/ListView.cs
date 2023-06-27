using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Lists
{
    public abstract class ListView<T> : IUiElement where T : IUiElement
    {
        private List<T> _oldItems;

        public ListView(UiEnvironment environment, List<T> items)
        {
            Environment = environment;

            Items = items;
            _oldItems = new List<T>(items);
        }

        public abstract int RequiredWidth { get; }

        public abstract int RequiredHeight { get; }

        public bool Changed { get; protected set; }

        public int Spacing { get; set; }

        public UiEnvironment Environment { get; }

        public List<T> Items { get; set; }

        public abstract Texture2D Render(Graphics graphics, int width, int height);

        public void Update(Vector2 position, int width, int height)
        {
            if (Items.Count != _oldItems.Count)
            {
                Changed = true;
            }
            else
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (!Items[i].Equals(_oldItems[i]))
                    {
                        Changed = true;
                        break;
                    }
                }
            }
            _oldItems = new List<T>(Items);
            UpdateItems(position, width, height);
        }

        protected abstract void UpdateItems(Vector2 position, int width, int height);
    }
}
