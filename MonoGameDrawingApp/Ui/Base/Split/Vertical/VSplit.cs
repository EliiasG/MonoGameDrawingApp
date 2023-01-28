using MonoGameDrawingApp.Ui.Split;
using System;

namespace MonoGameDrawingApp.Ui.Base.Split.Vertical
{
    public abstract class VSplit : BaseSplit
    {
        protected VSplit(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition) : base(environment, first, second, splitPosition) { }

        public override int RequiredWidth => Math.Max(First.RequiredWidth, Second.RequiredWidth);

        public override int RequiredHeight => First.RequiredHeight + Second.RequiredHeight;

        public override int MaxPosition => _height - Second.RequiredHeight;

        public override int MinPosition => First.RequiredHeight;
    }
}