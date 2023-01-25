using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Buttons;
using MonoGameDrawingApp.Ui.Tree.Trees;
using System.Diagnostics;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeView : IUiElement
    {
        public ITree Tree;

        public bool ItemClicked = false;

        private readonly UiEnvironment _environment;

        private Button _button;

        private bool _oldJustRight;
        private bool _oldJustLeft;

        public TreeView(UiEnvironment environment, int indentationAmount, int spacing, ITree tree, bool hideRoot)
        {
            Tree = tree;
            _environment = environment;
            _button = new Button(environment, new TreeItemView(environment, tree.Root, indentationAmount, spacing, hideRoot, this));
        }

        public bool Changed => _button.Changed;

        public int RequiredWidth => _button.RequiredWidth;

        public int RequiredHeight => _button.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            if(!ItemClicked && _oldJustLeft) 
            {
                Tree.BackgroundLeftClicked();
            }
            if (!ItemClicked && _oldJustRight)
            {
                Tree.BackgroundRightClicked();
            }

            ItemClicked = false;
            //wonky solution, _button.Update() resets the clicks, but we must set ItemClicked first
            _oldJustRight = _button.JustRightClicked;
            _oldJustLeft = _button.JustLeftClicked;
            _button.Update(position, width, height);
        }
    }
}
