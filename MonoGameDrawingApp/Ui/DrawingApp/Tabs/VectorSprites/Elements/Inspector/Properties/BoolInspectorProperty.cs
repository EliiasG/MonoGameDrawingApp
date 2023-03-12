using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties
{
    public class BoolInspectorProperty : IInspectorProperty<bool>
    {
        private const int ButtonSize = 24;

        private readonly CheckButton _checkButton;
        private readonly IUiElement _root;

        private bool _value;

        public BoolInspectorProperty(UiEnvironment environment, string name, bool value, Action changed)
        {
            Environment = environment;
            ValueChanged = changed;

            _checkButton = new CheckButton(Environment, value)
            {
                CheckedChanged = (bool newVal) =>
                {
                    Value = newVal;
                    ValueChanged?.Invoke();
                }
            };
            Value = value;

            _root = new NamedInspectorProperty(
                environment: Environment,
                child: new HSplitStandard(Environment, new EmptySpace(Environment, 1, 1), new MinSize(Environment, _checkButton, ButtonSize, ButtonSize), -2),
                text: name
            );
        }

        public bool Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    _checkButton.IsChecked = value;
                }
            }
        }

        public Action ValueChanged { get; set; }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; set; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }
    }
}
