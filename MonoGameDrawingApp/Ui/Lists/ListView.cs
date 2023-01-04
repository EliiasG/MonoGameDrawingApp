using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Lists
{
    public abstract class ListView<T> : IUiElement where T : IUiElement
    {
        public List<T> Items;
        private List<T> OldItems;

        protected bool _changed = true;

        public ListView(List<T> items) 
        {
            Items = items;
            OldItems = new List<T>(items);
        }

        public abstract int RequiredWidth { get; }

        public abstract int RequiredHeight { get; }

        public bool Changed => _changed;

        public abstract Texture2D Render(Graphics graphics, int width, int height);

        public void Update(Vector2 position, int width, int height)
        {
            if (Items.Count != OldItems.Count)
            {
                _changed = true;
            }
            else
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (!Items[i].Equals(OldItems[i]))
                    {
                        _changed = true;
                        break;
                    }
                }
            }
            OldItems = new List<T>(Items);
            _updateItems(position, width, height);
        }

        protected abstract void _updateItems(Vector2 position, int width, int height);
    }
}
