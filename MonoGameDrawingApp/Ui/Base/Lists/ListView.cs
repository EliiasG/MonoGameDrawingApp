using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Lists
{
    public abstract class ListView<T> : IUiElement where T : IUiElement
    {
        public List<T> Items;

        public int Spacing = 0;

        private readonly UiEnvironment _environment;

        private List<T> _oldItems;

        protected bool _changed = true;

        public ListView(UiEnvironment environment, List<T> items)
        {
            _environment = environment;

            Items = items;
            _oldItems = new List<T>(items);
        }

        public abstract int RequiredWidth { get; }

        public abstract int RequiredHeight { get; }

        public bool Changed => _changed;

        public UiEnvironment Environment => _environment;

        public abstract Texture2D Render(Graphics graphics, int width, int height);

        public void Update(Vector2 position, int width, int height)
        {
            if (Items.Count != _oldItems.Count)
            {
                _changed = true;
            }
            else
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (!Items[i].Equals(_oldItems[i]))
                    {
                        _changed = true;
                        break;
                    }
                }
            }
            _oldItems = new List<T>(Items);
            _updateItems(position, width, height);
        }

        protected abstract void _updateItems(Vector2 position, int width, int height);
    }
}
