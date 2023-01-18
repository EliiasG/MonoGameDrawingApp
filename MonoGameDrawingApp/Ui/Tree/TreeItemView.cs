using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Buttons;
using MonoGameDrawingApp.Ui.Lists;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using MonoGameDrawingApp.Ui.Split.Vertical;
using MonoGameDrawingApp.Ui.Tree.TreeItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeItemView : IUiElement
    {
        public readonly ITreeItem TreeItem;
        public readonly bool HideSelf;

        private readonly UiEnvironment _environment;

        private readonly HListView<IUiElement> _outer;
        private readonly VListView<TreeItemView> _childrenView;

        private readonly int _indentation;
        private readonly IUiElement _openButtonIcon;
        private readonly IUiElement _closedButtonIcon;
        private readonly IUiElement _defaultButtonIcon;
        private readonly ChangeableView _buttonIcon;
        private readonly Button _button;
        private readonly TextView _textView;
        private readonly Button _textButton;
        private readonly List<TreeItemView> _children;
        private readonly List<TreeItemView> _empty;
        private readonly ColorModifier _textColor;

        public TreeItemView(UiEnvironment environment, ITreeItem treeItem, int indentation, bool hideSelf)
        {
            _environment = environment;

            TreeItem = treeItem;
            _indentation = indentation;
            HideSelf = hideSelf;

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

            int buttonSize = (int) Environment.Font.MeasureString("X").Y;

            _empty = new List<TreeItemView>();
            _children = new List<TreeItemView>();
            _childrenView = new VListView<TreeItemView>(environment, _empty);
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

            _outer = new HListView<IUiElement>(environment, new List<IUiElement>
            {
                new MinSize(environment, new ColorRect(environment, Color.Transparent), indentation, 1),
                new VListView<IUiElement>(environment, new List<IUiElement>
                {
                    new HListView<IUiElement>(environment, new List<IUiElement>
                    {
                        new MinSize(environment,new ColorModifier(environment, new ScaleView(environment, icon), environment.Theme.DefaultTextColor), buttonSize, buttonSize),
                        _textButton,
                        _button,
                    }),
                    _childrenView,
                }),
            });
        }

        public int Indentation
        {
            get => HideSelf ? 0 : _indentation;
        }

        public int RequiredWidth => _outer.RequiredWidth;

        public int RequiredHeight => _outer.RequiredHeight;

        public bool Changed => _outer.Changed;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
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

            _outer.Update(position, width, height);
        }
    }
}
