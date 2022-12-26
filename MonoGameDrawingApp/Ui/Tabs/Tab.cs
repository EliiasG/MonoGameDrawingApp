using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public abstract class Tab
    {
        public readonly IUiElement Child;
        public abstract bool HasCloseButton { get; }
        public abstract string Title { get; }
        public TabBar TabBar;
        public bool IsSelected = false;

        protected abstract void _closeButtonPressed();

        public void Close()
        {
            TabBar.CloseTab(this);
        }
    }
}
