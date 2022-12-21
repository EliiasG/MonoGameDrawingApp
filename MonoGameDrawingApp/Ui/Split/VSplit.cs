using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Split
{
    public abstract class VSplit : Split
    {
        protected VSplit(IUiElement first, IUiElement second, int splitPosition) : base(first, second, splitPosition) {}

        public override int RequiredWidth => Math.Max(First.RequiredWidth, Second.RequiredWidth);

        public override int RequiredHeight => First.RequiredHeight + Second.RequiredHeight;

        public override int MaxPosition => _height - Second.RequiredHeight;

        public override int MinPosition => First.RequiredHeight;
    }
}