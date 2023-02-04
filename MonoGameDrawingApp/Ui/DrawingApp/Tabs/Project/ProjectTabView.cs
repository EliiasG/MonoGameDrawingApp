using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.Tree;
using MonoGameDrawingApp.Ui.FileSystemTrees;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.Project
{
    public class ProjectTabView : IUiElement
    {
        public readonly string Path;

        private IUiElement _root;

        public ProjectTabView(UiEnvironment environment, DrawingAppRoot root, string path)
        {
            Environment = environment;
            Path = path;

            FileSystemTree tree = new FileSystemTree(Path, root.PopupEnvironment, root.FileTypeManager, true, true);

            _root = new StackView(environment, new IUiElement[]
            {
                new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                new ScrollWindow(Environment, new TreeView(Environment, 20, 1, tree, false))
            });
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
            _root.Update(position, width, height);
        }
    }
}
