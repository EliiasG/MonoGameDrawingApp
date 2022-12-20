using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Split
{
    public abstract class Split : IUiElement
    {
        public IUiElement First;
        public IUiElement Second;
        private int _splitPosition;
        protected int _height;
        protected int _width;
        public Split(IUiElement first, IUiElement second, int splitPosition)
        {
            SplitPosition = splitPosition;
            First = first;
            Second = second;
        }

        public int SplitPosition {
            set => _splitPosition = value;
            get => Math.Clamp(_splitPosition, MinPosition, MaxPosition); //clamping in get because Min and Max position may change between setting
        }

        public abstract int RequiredWidth { get; }

        public abstract int RequiredHeight { get; }

        public abstract int MaxPosition { get; }

        public abstract int MinPosition { get; }

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            _height = height;
            _width = width;
            return _render(graphics, position);
        }

        protected abstract Texture2D _render(Graphics graphics, Vector2 position);
    }
}
