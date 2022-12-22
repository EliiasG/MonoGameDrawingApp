using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Split.Horizontal
{
    public abstract class HSplit : Split
    {
        protected HSplit(IUiElement first, IUiElement second, int splitPosition) : base(first, second, splitPosition) { }

        public override int RequiredHeight => Math.Max(First.RequiredHeight, Second.RequiredHeight);

        public override int RequiredWidth => First.RequiredWidth + Second.RequiredWidth;

        public override int MaxPosition => _width - Second.RequiredWidth;

        public override int MinPosition => First.RequiredWidth;
    }
}
