using MonoGameDrawingApp.Ui.Split.Horizontal;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public class HScrollBar : ScrollBar
    {
        public HScrollBar()
        {
            _inner = new HSplitStandard(Bar, SecondBackground, 1);
            _outer = new HSplitStandard(FirstBackground, _inner, 1);
        }
    }
}
