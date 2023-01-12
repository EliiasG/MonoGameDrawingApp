using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Buttons;
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
        public readonly bool HideSelf;

        private readonly UiEnvironment _environment;

        private readonly int _indentation;
        private readonly HSplit _outer;
        private readonly HSplit _inner;
        private readonly HSplit _title;
        private readonly VSplit _split;
        private readonly VListView<TreeItemView> _childrenView;
        private readonly IUiElement _openButtonIcon;
        private readonly IUiElement _closedButtonIcon;
        private readonly IUiElement _defaultButtonIcon;
        private readonly ChangeableView _buttonIcon;
        private readonly Button _button;
        private readonly ChangeableView _icon;
        private readonly TextView _textView;
        private readonly Button _textButton;
        private readonly List<TreeItemView> _children;
        private readonly List<TreeItemView> _empty;

        public TreeItemView(UiEnvironment environment, ITreeItem treeItem, int indentation, bool hideSelf)
        {
            _environment = environment;

            TreeItem = treeItem;
            _indentation = indentation;
            HideSelf = hideSelf;
            int size = (int)Math.Ceiling(Environment.Font.MeasureString("X").Y);

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
            _childrenView = new VListView<TreeItemView>(environment, _children);
            _textView = new TextView(environment, TreeItem.Name);
            _openButtonIcon = new SpriteView(environment, "icons/open");
            _closedButtonIcon = new SpriteView(environment, "icons/closed");
            _defaultButtonIcon = new ColorRect(environment, Color.Transparent);
            _icon = new ChangeableView(environment, new SpriteView(environment, treeItem.IconPath));
            _textButton = new Button(environment, _textView);
            _title = new HSplitStandard(environment, new MinSize(environment, new ScaleView(environment, _icon), size, size), _textButton, 0);
            _buttonIcon = new ChangeableView(environment, _defaultButtonIcon);
            _button = new Button(environment, _buttonIcon);
            _inner = new HSplitStandard(environment, _title, new MinSize(environment, new ScaleView(environment, _button), size, size), 0);
            _split = new VSplitStandard(environment, _inner, HideSelf ? new ColorRect(environment, Color.Transparent) : _childrenView, 0);
            _outer = new HSplitStandard(environment, new ColorRect(environment, Color.Transparent), _split, 0);
        }

        public int Indentation 
        {
            get => HideSelf ? 0 : _indentation;
        }

        public int RequiredWidth => _inner.RequiredWidth + Indentation + 1; //_inner is intentional, _outer.Left will just be blank to add space

        public int RequiredHeight => _outer.RequiredHeight;

        public bool Changed => _outer.Changed;

        public UiEnvironment Environment => _environment;

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
                       _children.Add(new TreeItemView(Environment, child, _indentation, false)); //_indentation, because it should use the value given in the constructor
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
