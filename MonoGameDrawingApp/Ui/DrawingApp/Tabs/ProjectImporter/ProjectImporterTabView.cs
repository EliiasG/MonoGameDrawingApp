﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;
using MonoGameDrawingApp.Ui.Base.Tree;
using MonoGameDrawingApp.Ui.FileSystemTrees;
using MonoGameDrawingApp.Ui.FileSystemTrees.Items;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.ProjectImporter
{
    public class ProjectImporterTabView : IUiElement
    {
        private readonly FileSystemTree _tree;
        private readonly VSplit _root;
        private readonly ContextMenuButton _importButton;

        public ProjectImporterTabView(UiEnvironment environment, DrawingAppStart start, ProjectImporterTab tab)
        {
            Start = start;
            Tab = tab;
            Environment = environment;
            _tree = new FileSystemTree("", start.Root.PopupEnvironment, start.Root.FileTypeManager, true, false);
            _importButton = new ContextMenuButton(Environment, "Import", Import);
            _root = new VSplitStandard(
                environment: Environment,
                first: new StackView(Environment, new List<IUiElement>() {
                    new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                    new ScrollWindow(Environment, new TreeView(Environment, 20, 1, _tree, true)),
                }),
                second: new HListView<IUiElement>(environment, new List<IUiElement>()
                {
                    _importButton,
                    new EmptySpace(Environment,  10, 1),
                    new ContextMenuButton(Environment, "Close", tab.Close),
                }),
                splitPosition: 1
            );
        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public DrawingAppStart Start { get; }

        public ProjectImporterTab Tab { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.SplitPosition = height;
            _importButton.Disabled = _tree.Selected == null;
            _root.Update(position, width, height);
        }

        private void Import()
        {
            if (_tree.Selected is DirectoryTreeItem item)
            {
                Tab.Close();
                SaveState.SetFirst(item.Path);
                Start.ReloadProjects();
            }
        }
    }
}
