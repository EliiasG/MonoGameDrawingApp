using MonoGameDrawingApp.Ui.Tree.TreeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Tree
{
    public interface ITree
    {
        ITreeItem Root { get; }

        bool HideRoot { get; }
    }
}
