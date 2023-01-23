using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Tree.Trees;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeView : IUiElement
    {
        public ITree Tree;

        public bool ItemClicked = false;

        private MouseState _oldMouse;
        private readonly UiEnvironment _environment;

        private TreeItemView _treeItemView;

        public TreeView(UiEnvironment environment, int indentationAmount, int spacing, ITree tree, bool hideRoot)
        {
            Tree = tree;
            _environment = environment;
            _treeItemView = new TreeItemView(environment, tree.Root, indentationAmount, spacing, hideRoot, this);
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
            MouseState mouse = Mouse.GetState();
            ItemClicked = false;
            _treeItemView.Update(position, width, height);

            if(!ItemClicked && mouse.LeftButton == ButtonState.Pressed && _oldMouse.LeftButton != ButtonState.Pressed) 
            {
                Tree.BackgroundLeftClicked();
            }
            if (!ItemClicked && mouse.RightButton == ButtonState.Pressed && _oldMouse.RightButton != ButtonState.Pressed)
            {
                Tree.BackgroundRightClicked();
            }

            _oldMouse = Mouse.GetState();
        }
    }
}
