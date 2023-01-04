using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Lists;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using MonoGameDrawingApp.Ui.Tree.TreeItems;
using System;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeItemView : IUiElement
    {
        public readonly ITreeItem TreeItem;
        public readonly SpriteFont Font;
        public int Indentation;

        private readonly HSplit _outer;
        private readonly HSplit _inner;
        private readonly HSplit _title;
        private readonly IUiElement _openButtonIcon;
        private readonly IUiElement _closeButtonIcon;
        private readonly IUiElement _defaultButtonIcon;
        private readonly IUiElement _defaultIcon;
        private readonly ChangeableView _buttonIcon;
        private readonly Button _button;
        private readonly ChangeableView _icon;
        private readonly TextView _textView;
        private readonly Button _textButton;

        public TreeItemView(SpriteFont spriteFont, ITreeItem treeItem, SpriteFont font, int indentation)
        {
            TreeItem = treeItem;
            Font = font;
            int size = (int)Math.Ceiling(Font.MeasureString("X").Y);
            /* It's a bit hard to see the structure from the code, here is a simplified version:
             * _outer:
             *   Nothing
             *   _inner:
             *     _title:
             *       _icon
             *       _textButton:
             *         _textView
             *     _button:
             *       _buttonIcon
            */
            _textView = new TextView(spriteFont, TreeItem.Name);
            _openButtonIcon = new SpriteView("icons/open");
            _closeButtonIcon = new SpriteView("icons/close");
            _defaultIcon = new ColorRect(Color.Transparent);
            _defaultButtonIcon = new ColorRect(Color.Transparent);
            _icon = new ChangeableView(_defaultIcon);
            _textButton = new Button(_textView);
            _title = new HSplitStandard(new MinSize(new ScaleView(_icon), size, size), _textButton, 0);
            _buttonIcon = new ChangeableView(_defaultButtonIcon);
            _button = new Button(_buttonIcon);
            _inner = new HSplitStandard(_title, new MinSize(new ScaleView(_button), size, size), 0);
            _outer = new HSplitStandard(new ColorRect(Color.Transparent), _inner, 0);


            Indentation = indentation;
        }

        public int RequiredWidth => _inner.RequiredWidth + Indentation; //_inner is intentional, left will just be blank to add space

        public int RequiredHeight => _outer.RequiredHeight;

        public bool Changed => _outer.Changed;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _textView.Text = TreeItem.Name;

            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
