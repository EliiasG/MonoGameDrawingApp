using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Popup;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties
{
    public class ColorInspectorProperty : IInspectorProperty<System.Drawing.Color>
    {
        private readonly IUiElement _root;
        private readonly ColorRect _colorRect;
        private readonly Button _button;

        private System.Drawing.Color _value;

        public ColorInspectorProperty(UiEnvironment environment, PopupEnvironment popupEnvironment, string name, System.Drawing.Color value, Action changed)
        {
            Environment = environment;

            _colorRect = new ColorRect(Environment, Color.Transparent);

            _button = new Button(
                environment: Environment,
                child: new MinSize(Environment, _colorRect, 100, 30)
            );

            ValueChanged = changed;

            _root = new NamedInspectorProperty(Environment, _button, name);

            Value = value;

            PopupEnvironment = popupEnvironment;
        }

        public System.Drawing.Color Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    _colorRect.Color = Util.ToXnaColor(value);
                }
            }
        }

        public PopupEnvironment PopupEnvironment { get; set; }

        public Action ValueChanged { get; set; }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            if (_button.JustLeftClicked)
            {
                ColorPickerPopup popup = new(Environment, PopupEnvironment, Value, (c) =>
                {
                    Value = c;
                    ValueChanged?.Invoke();
                });
                PopupEnvironment.OpenCentered(popup);
            }

            _root.Update(position, width, height);
        }
    }
}
