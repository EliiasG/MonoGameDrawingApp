using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.Tree
{
    public class TreeItemView : IUiElement
    {
        public readonly ITreeItem TreeItem;
        public readonly TreeView TreeView;
        public readonly bool IndentChildren;

        private bool _childrenChanged = true;
        private bool _wasOpen = false;

        private readonly UiEnvironment _environment;

        private readonly HListView<IUiElement> _outer;
        private readonly List<TreeItemView> _empty;
        private readonly int _indentation;
        private readonly IUiElement _openButtonIcon;
        private readonly IUiElement _closedButtonIcon;
        private readonly IUiElement _defaultButtonIcon;
        private readonly ChangeableView _buttonIcon;
        private readonly Button _button;
        private readonly Button _whole;
        private readonly TextView _textView;
        private readonly Button _textButton;
        private readonly List<TreeItemView> _children;
        private readonly ColorModifier _textColor;

        public TreeItemView(UiEnvironment environment, ITreeItem treeItem, int indentation, TreeView treeView, bool indentChildren)
        {
            _environment = environment;

            TreeView = treeView;
            TreeItem = treeItem;
            _indentation = indentation;

            _empty = new List<TreeItemView>();
            _children = new List<TreeItemView>();
            IndentChildren = indentChildren;

            /* It's a bit hard to see the structure from the code, here is a simplified version:
             * _outer:
             *   Nothing
             *   VListView:
             *     HListView:
             *       MinSize:
             *         ScaleView:
             *           icon
             *       _textButton:
             *         _textColor:
             *           _textView
             *       _button:
             *         _buttonIcon
             *     _childrenView
            */

            int buttonSize = (int)Environment.FontHeight;

            _textView = new TextView(environment, " " + TreeItem.Name);
            _textColor = new ColorModifier(environment, _textView, environment.Theme.DefaultTextColor);
            _openButtonIcon = new SpriteView(environment, "icons/open");
            _closedButtonIcon = new SpriteView(environment, "icons/closed");
            _defaultButtonIcon = new ColorRect(environment, Color.Transparent);
            _textButton = new Button(environment, _textColor);
            _buttonIcon = new ChangeableView(environment, _defaultButtonIcon);
            _button = new Button(
                environment: environment,
                child: new MinSize(
                    environment: environment,
                    child: new ScaleView(
                        environment: environment,
                        child: new ColorModifier(
                            environment: environment,
                            child: _buttonIcon,
                            color: environment.Theme.ButtonColor
                        )
                    ),
                    width: buttonSize,
                    height: buttonSize
                )
            );

            IUiElement icon = treeItem.IconPath == null ? new ColorRect(Environment, Color.Transparent) : new SpriteView(environment, treeItem.IconPath);

            _whole = new Button(Environment, new HListView<IUiElement>(environment, new List<IUiElement>
            {
                _button,
                new MinSize(environment,new ColorModifier(environment, new ScaleView(environment, icon), environment.Theme.DefaultTextColor), buttonSize, buttonSize),
                _textButton,
            }));

            _outer = new HListView<IUiElement>(environment, new List<IUiElement>
            {
                new MinSize(environment, new ColorRect(environment, Color.Transparent), Indentation, 1),
                _whole
            });
        }

        public int Indentation
        {
            get => _indentation;
        }

        public int RequiredWidth => _outer.RequiredWidth;

        public int RequiredHeight => _outer.RequiredHeight;

        public bool Changed => _outer.Changed;

        public bool ChildrenChanged => _childrenChanged;

        public List<TreeItemView> Children => TreeItem.IsOpen ? _children : _empty;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        private bool _calculateChildrenChanged(IEnumerable<ITreeItem> itemChildren)
        {
            if (itemChildren.Count() != _children.Count)
            {
                return true;
            }
            int i = 0;
            foreach (ITreeItem itemChild in itemChildren)
            {
                if (itemChild != _children[i].TreeItem)
                {
                    return true;
                }
                ++i;
            }
            return false;

        }

        public void Update(Vector2 position, int width, int height)
        {
            /*
            _outer.SplitPosition = Indentation + 1;
            _inner.SplitPosition = 10;
            _split.SplitPosition = 10;
            */

            _textColor.Color = TreeItem.Tree.Selected == TreeItem ? Environment.Theme.HoveringTextColor : Environment.Theme.DefaultTextColor;
            _textView.Text = " " + TreeItem.Name;
            _buttonIcon.Child = TreeItem.HasOpenButton ? TreeItem.IsOpen ? _openButtonIcon : _closedButtonIcon : _defaultButtonIcon;

            if (_textButton.JustLeftClicked)
            {
                TreeItem.Clicked();
            }

            if (_textButton.JustRightClicked)
            {
                TreeItem.RightClicked();
            }

            if (_whole.JustLeftClicked)
            {
                TreeView.ItemClicked = true;
            }

            if (_whole.JustRightClicked)
            {
                TreeView.ItemClicked = true;
            }

            _outer.Update(position, width, height);
        }

        public void UpdateChildren()
        {
            if (TreeItem.IsOpen)
            {
                foreach (TreeItemView child in _children)
                {
                    child.UpdateChildren();
                }
                IEnumerable<ITreeItem> itemChildren = TreeItem.Children; // TreeItem.Children can be expensive

                _childrenChanged = _calculateChildrenChanged(itemChildren);
                if (_childrenChanged)
                {
                    foreach (TreeItemView child in new List<TreeItemView>(_children)) // loop may modify _children
                    {
                        if (!itemChildren.Any((item) => child.TreeItem == item)) // none of the children of the treeitem contain the current child
                        {
                            _children.Remove(child);
                        }
                    }

                    foreach (ITreeItem treeItem in itemChildren)
                    {
                        if (!_children.Any((child) => child.TreeItem == treeItem)) // none of the children are assigned to the current item
                        {
                            _children.Add(new TreeItemView(Environment, treeItem, IndentChildren ? Indentation + TreeView.IndentationAmount : Indentation, TreeView, true));
                        }
                    }

                    ITreeItem[] childArray = itemChildren.ToArray();
                    _children.Sort((v1, v2) => Array.IndexOf(childArray, v1.TreeItem) - Array.IndexOf(childArray, v2.TreeItem));
                }

            }
            else
            {
                _childrenChanged = _wasOpen;
            }

            if (_button.JustLeftClicked)
            {
                TreeItem.IsOpen = !TreeItem.IsOpen;
                _childrenChanged = true;
            }

            _childrenChanged = _childrenChanged || Children.Any((item) => item.ChildrenChanged);
            _wasOpen = TreeItem.IsOpen;
        }
    }
}
