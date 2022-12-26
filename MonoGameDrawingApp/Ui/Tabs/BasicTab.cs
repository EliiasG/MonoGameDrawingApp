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

        public BasicTab(string title)
        {
            _title = title;
        }

        public override bool HasCloseButton => true;

        public override string Title => _title;

        protected override void _closeButtonPressed()
        {
            Close();
        }
    }
}
