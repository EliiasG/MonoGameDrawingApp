using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Lists;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using MonoGameDrawingApp.Ui.Split.Vertical;
using MonoGameDrawingApp.Ui.Tree.TreeItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeItemView : IUiElement
    {
        public readonly ITreeItem TreeItem;
        public readonly SpriteFont Font;
        public readonly bool HideSelf;

        private readonly int _indentation;
        private readonly HSplit _outer;
        private readonly HSplit _inner;
        private readonly HSplit _title;
        private readonly VSplit _split;
        private readonly VListView<TreeItemView> _childrenView;
        private readonly IUiElement _openButtonIcon;
        private readonly IUiElement _closedButtonIcon;
        private readonly IUiElement _defaultButtonIcon;
        private readonly IUiElement _defaultIcon;
        private readonly ChangeableView _buttonIcon;
        private readonly Button _button;
        private readonly ChangeableView _icon;
        private readonly TextView _textView;
        private readonly Button _textButton;
        private readonly List<TreeItemView> _children;
        private readonly List<TreeItemView> _empty;

        public TreeItemView(SpriteFont font, ITreeItem treeItem, int indentation, bool hideSelf)
        {
            TreeItem = treeItem;
            Font = font;
            _indentation = indentation;
            HideSelf = hideSelf;
            int size = (int)Math.Ceiling(Font.MeasureString("X").Y);

            /* It's a bit hard to see the structure from the code, here is a simplified version:
             * _outer:
             *   Nothing
             *   _split:
             *     _inner:
             *       _title:
             *         _icon
             *         _textButton:
             *           _textView
             *       _button:
             *         _buttonIcon
             *     _childrenView
             *     
            */
            
            _empty = new List<TreeItemView>();
            _children = new List<TreeItemView>();
            _childrenView = new VListView<TreeItemView>(_children);
            _textView = new TextView(Font, TreeItem.Name);
            _openButtonIcon = new SpriteView("icons/open");
            _closedButtonIcon = new SpriteView("icons/closed");
            _defaultIcon = new ColorRect(Color.Transparent);
            _defaultButtonIcon = new ColorRect(Color.Transparent);
            _icon = new ChangeableView(_defaultIcon);
            _textButton = new Button(_textView);
            _title = new HSplitStandard(new MinSize(new ScaleView(_icon), size, size), _textButton, 0);
            _buttonIcon = new ChangeableView(_defaultButtonIcon);
            _button = new Button(_buttonIcon);
            _inner = new HSplitStandard(_title, new MinSize(new ScaleView(_button), size, size), 0);
            _split = new VSplitStandard(_inner, HideSelf ? new ColorRect(Color.Transparent) : _childrenView, 0);
            _outer = new HSplitStandard(new ColorRect(Color.Transparent), _split, 0);
        }

        public int Indentation 
        {
            get => HideSelf ? 0 : _indentation;
        }

        public int RequiredWidth => _inner.RequiredWidth + Indentation + 1; //_inner is intentional, _outer.Left will just be blank to add space

        public int RequiredHeight => _outer.RequiredHeight;

        public bool Changed => _outer.Changed;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _outer.SplitPosition = Indentation + 1;
            _inner.SplitPosition = 0;
            _split.SplitPosition = 0;
            _textView.Text = TreeItem.Name;
            _buttonIcon.Child = TreeItem.HasOpenButton ? (TreeItem.IsOpen ? _openButtonIcon : _closedButtonIcon) : _defaultButtonIcon;
            _icon.Child = TreeItem.Icon ?? _defaultIcon;

            if(TreeItem.IsOpen) 
            { 
                foreach (TreeItemView child in _children)
                {
                    if(!TreeItem.Children.Contains(child.TreeItem))
                    {
                        _children.Remove(child);
                    }
                }

                foreach (ITreeItem child in TreeItem.Children)
                {
                    if(_children.All(item => item.TreeItem != child)) //none of the children contain the current child
                    {
                        _children.Add(new TreeItemView(Font, child, _indentation, false)); //_indentation, because it should use the value given in the constructor
                    }
                }

                _childrenView.Items = _children;
            }
            else
            {
                _childrenView.Items = _empty;
            }

            _outer.Update(position, width, height);

            if (_textButton.JustLeftClicked)
            {
                TreeItem.Clicked();
            }

            if (_textButton.JustRightClicked)
            {
                TreeItem.RightClicked();
            }

            if (_button.JustLeftClicked)
            {
                TreeItem.IsOpen = !TreeItem.IsOpen;
            }
        }
    }
}
