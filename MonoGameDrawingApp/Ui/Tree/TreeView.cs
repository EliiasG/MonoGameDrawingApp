using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeView : IUiElement
    {
        private TreeItemView _treeItemView;

        public TreeView(SpriteFont font, int indentationAmount, ITree tree)
        {
            _treeItemView = new TreeItemView(font, tree.Root, indentationAmount, tree.HideRoot);
        }

        public bool Changed => _treeItemView.Changed;

        public int RequiredWidth => _treeItemView.RequiredWidth;

        public int RequiredHeight => _treeItemView.RequiredHeight;

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
