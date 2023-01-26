using System;

namespace MonoGameDrawingApp.Ui.Popup
{
    public struct ChoicePopupOption
    {
        public ChoicePopupOption(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }

        public string Name { get; init; }
        public Action Selected { get; init; }
    }
}
