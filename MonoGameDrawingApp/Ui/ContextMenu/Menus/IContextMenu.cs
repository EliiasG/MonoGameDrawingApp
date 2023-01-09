using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.ContextMenu.Menus
{
    public interface IContextMenu
    {
        IEnumerable<IUiElement> Children { get; }
    }
}
