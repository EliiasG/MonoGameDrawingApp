using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Base.Split
{
    public abstract class BaseSplit : IUiElement
    {
        private int _splitPosition;

        private bool _stick;
        private bool _stickToLast;
        private bool _stickToMiddle;

        public BaseSplit(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition)
        {
            Environment = environment;

            First = first;
            Second = second;
            SplitPosition = splitPosition;
        }

        public int SplitPosition
        {
            set
            {
                _stick = value < 0;
                _stickToLast = value == -2;
                _stickToMiddle = value == -3;
                SetSplitPositon(value);
            }
            get
            {
                if (_stick)
                {
                    if (_stickToMiddle)
                    {
                        _splitPosition = Math.Max(MaxPosition, MinPosition) / 2;
                    }
                    else
                    {
                        _splitPosition = _stickToLast ? Math.Max(Width, Height) : 0;
                    }
                }
                SetSplitPositon(_splitPosition); //to update the split position, as it may been restricted since set
                return _splitPosition;
            }
        }

        private void SetSplitPositon(int splitPosition)
        {
            if (Width == -1)
            {
                _splitPosition = splitPosition;
                Changed = true;
                return;
            }
            int newSplitPosition = Math.Clamp(splitPosition, MinPosition, Math.Max(MaxPosition, MinPosition));
            if (newSplitPosition != _splitPosition)
            {
                Changed = true;
                _splitPosition = newSplitPosition;
            }
        }

        public abstract int RequiredWidth { get; }

        public abstract int RequiredHeight { get; }

        public abstract int MaxPosition { get; }

        public abstract int MinPosition { get; }

        public bool Changed { get; protected set; }

        public UiEnvironment Environment { get; }
        protected int Width { get; set; } = -1;
        protected int Height { get; set; } = -1;

        public IUiElement Second { get; }

        public IUiElement First { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {

            Height = height;
            Width = width;
            Texture2D res = Render(graphics);
            Changed = false;
            return res;
        }

        protected abstract Texture2D Render(Graphics graphics);

        public abstract void Update(Vector2 position, int width, int height);
    }
}