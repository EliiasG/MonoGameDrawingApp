using MonoGameDrawingApp.Ui.Split.Vertical;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public class VScrollBar : ScrollBar
    {
        public VScrollBar() : base()
        {
            _inner = new VSplitStandard(Bar, SecondBackground, 1);
            _outer = new VSplitStandard(FirstBackground, _inner, 1);
        }
    }
}
