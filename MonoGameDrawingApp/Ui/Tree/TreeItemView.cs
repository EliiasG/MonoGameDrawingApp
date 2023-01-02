using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Tree.TreeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeItemView : IUiElement
    {
        public ITreeItem TreeItem;

        private readonly TextView _textView;

        public TreeItemView(SpriteFont spriteFont, ITreeItem treeItem)
        {
            TreeItem = treeItem;
            _textView = new TextView(spriteFont, TreeItem.Name);
        }

        public int RequiredWidth => throw new NotImplementedException();

        public int RequiredHeight => throw new NotImplementedException();

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            _textView.Text = TreeItem.Name;

            throw new NotImplementedException();
        }
    }
}
