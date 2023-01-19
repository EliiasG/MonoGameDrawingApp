using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Tree.Trees;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeView : IUiElement
    {

        private readonly UiEnvironment _environment;

        private TreeItemView _treeItemView;

        public TreeView(UiEnvironment environment, int indentationAmount, int spacing, ITree tree)
        {
            _environment = environment;
            _treeItemView = new TreeItemView(environment, tree.Root, indentationAmount, spacing, tree.HideRoot);
        }

        public bool Changed => _treeItemView.Changed;

        public int RequiredWidth => _treeItemView.RequiredWidth;

        public int RequiredHeight => _treeItemView.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _treeItemView.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _treeItemView.Update(position, width, height);
        }
    }
}
