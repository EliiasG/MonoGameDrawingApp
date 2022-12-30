using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class BasicTab : Tab
    {
        private readonly string _title;
        private readonly IUiElement _child;

        public BasicTab(string title, IUiElement child)
        {
            _title = title;
            _child = child;
        }

        public override bool HasCloseButton => true;

        public override string Title => _title;

        public override IUiElement Child => _child;

        protected override void _closeButtonPressed()
        {
            Close();
        }
    }
}
