using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using System;

namespace MonoGameDrawingApp.Ui.Base.Split
{
    public abstract class BaseSplit : IUiElement
    {
        public readonly IUiElement First;
        public readonly IUiElement Second;

        private readonly UiEnvironment _environment;

        private int _splitPosition;

        protected int _height = -1;
        protected int _width = -1;
        protected bool _changed;


        public BaseSplit(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition)
        {
            _environment = environment;

            First = first;
            Second = second;
            SplitPosition = splitPosition;
        }

        public int SplitPosition
        {
            set
            {
                setSplitPositon(value);
            }
            get
            {
                setSplitPositon(_splitPosition); //to update the split position, as it may been restricted since set
                return _splitPosition;
            }
        }

        private void setSplitPositon(int splitPosition)
        {
            if (_width == -1)
            {
                _splitPosition = splitPosition;
                _changed = true;
                return;
            }
            int newSplitPosition = Math.Clamp(splitPosition, MinPosition, Math.Max(MaxPosition, MinPosition));
            if (newSplitPosition != _splitPosition)
            {
                _changed = true;
                _splitPosition = newSplitPosition;
            }
        }

        public abstract int RequiredWidth { get; }

        public abstract int RequiredHeight { get; }

        public abstract int MaxPosition { get; }

        public abstract int MinPosition { get; }

        public bool Changed => _changed;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {

            _height = height;
            _width = width;
            Texture2D res = _render(graphics);
            _changed = false;
            return res;
        }

        protected abstract Texture2D _render(Graphics graphics);

        public abstract void Update(Vector2 position, int width, int height);
    }
}