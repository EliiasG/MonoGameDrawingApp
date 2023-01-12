﻿using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Tree.TreeItems
{
    public interface ITreeItem
    {
        string Name { get; }

        bool IsOpen { get; set; }

        bool HasOpenButton { get; }

        string IconPath { get; }

        ITree Tree { get; }

        void Clicked();

        void RightClicked();

        IEnumerable<ITreeItem> Children { get; }
    }
}
