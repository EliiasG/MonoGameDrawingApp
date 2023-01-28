using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Buttons;
using MonoGameDrawingApp.Ui.List;
using MonoGameDrawingApp.Ui.Scroll;
using MonoGameDrawingApp.Ui.Tree.Trees;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeView : IScrollableView
    {
        public readonly bool HideRoot;

        public ITree Tree;
        public bool ItemClicked = false;
        public int IndentationAmount;

        private readonly UiEnvironment _environment;
        private readonly Button _button;
        private readonly TreeItemView _rootView;
        private readonly VScrollableListView _listView;

        private List<TreeItemView> _items;
        private bool _oldJustRight;
        private bool _oldJustLeft;

        public TreeView(UiEnvironment environment, int indentationAmount, int spacing, ITree tree, bool hideRoot)
        {
            Tree = tree;
            _environment = environment;
            _rootView = new TreeItemView(Environment, tree.Root, 1, this, !hideRoot);
            _items = new List<TreeItemView>();
            _listView = new VScrollableListView(environment, _items, false, 3);
            _button = new Button(environment, _listView);
            HideRoot = hideRoot;
            IndentationAmount = indentationAmount;
        }

        public bool Changed => _button.Changed;

        public int RequiredWidth => _button.RequiredWidth;

        public int RequiredHeight => _button.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Vector2 Position { get => _listView.Position; set => _listView.Position = value; }

        public int Width => _listView.Width;

        public int Height => _listView.Height;

        public int MaxWidth => _listView.MaxWidth;

        public int MaxHeight => _listView.MaxHeight;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _updateItems();

            if (!ItemClicked && _oldJustLeft)
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

        private void _updateItems()
        {
            if (HideRoot)
            {
                Tree.Root.IsOpen = true;
            }

            _rootView.UpdateChildren();
            if (_rootView.ChildrenChanged)
            {
                _items.Clear();
                if (!HideRoot)
                {
                    _items.Add(_rootView);
                }
                _addChildren(_rootView);
            }
        }

        private void _addChildren(TreeItemView item)
        {
            foreach (TreeItemView child in item.Children)
            {
                _items.Add(child);
                _addChildren(child);
            }
        }
    }
}
