using MonoGameDrawingApp.Ui.Base;
using System;

namespace MonoGameDrawingApp.Ui.Split.Horizontal
{
    public abstract class HSplit : BaseSplit
    {
        protected HSplit(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition) : base(environment, first, second, splitPosition) { }

        public override int RequiredHeight => Math.Max(First.RequiredHeight, Second.RequiredHeight);

        public override int RequiredWidth => First.RequiredWidth + Second.RequiredWidth;

        public override int MaxPosition => _width - Second.RequiredWidth;

        public override int MinPosition => First.RequiredWidth;
    }
}
