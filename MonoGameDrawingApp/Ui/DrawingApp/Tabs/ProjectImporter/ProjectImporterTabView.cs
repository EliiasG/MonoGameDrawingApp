using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenu.Items;
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
        public readonly DrawingAppStart Start;
        public readonly ProjectImporterTab Tab;

        private FileSystemTree _tree;
        private VSplit _root;
        private ContextMenuButton _importButton;

        public ProjectImporterTabView(UiEnvironment environment, DrawingAppStart start, ProjectImporterTab tab)
        {
            Start = start;
            Tab = tab;
            Environment = environment;
            _tree = new FileSystemTree("", start.Root.PopupEnvironment, start.Root.FileTypeManager, true, false);
            _importButton = new ContextMenuButton(Environment, "Import", _import);
            _root = new VSplitStandard(
                environment: Environment,
                first: new StackView(Environment, new List<IUiElement>() {
                    new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                    new ScrollWindow(Environment, new TreeView(Environment, 20, 1, _tree, true)),
                }),
                second: new HListView<IUiElement>(environment, new List<IUiElement>()
                {
                    _importButton,
                    new MinSize(Environment, new ColorRect(Environment, Color.Transparent), 10, 1),
                    new ContextMenuButton(Environment, "Close", tab.Close),
                }),
                splitPosition: 1
            );
        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; init; }

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

        private void _import()
        {
            if (_tree.Selected is DirectoryTreeItem item)
            {
                Tab.Close();
                Start.SetFirst(item.Path);
            }
        }
    }
}
