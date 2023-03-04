using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.TextInput;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Properties
{
    public class StringInspectorProperty : IInspectorProperty<string>
    {
        private const int Height = 30;
        private const int TextWidth = 200;

        private readonly IUiElement _root;

        private string _value;

        public StringInspectorProperty(UiEnvironment environment, string name, string value, ITextInputFilter[] textInputFilters, Action changed)
        {
            ValueChanged = changed;
            Environment = environment;

            TextInput = new TextInputField(Environment, value, textInputFilters);
            Value = value;

            TextInput.TextEntered += () =>
            {
                Value = TextInput.Value;
                ValueChanged?.Invoke();
            };

            TextInput.Deselected += () =>
            {
                Value = TextInput.Value;
                ValueChanged?.Invoke();
            };

            ScrollWindow scrollWindow = new(
                environment: Environment,
                child: new PeekView(
                    environment: Environment,
                    child: TextInput
                )
            )
            {
                ScrollBarSize = 5
            };

            _root = new NamedInspectorProperty(
                environment: Environment,
                child: new MinSize(
                    environment: Environment,
                    child: scrollWindow,
                    width: TextWidth,
                    height: Height
                ),
                text: name
            );
        }

        public TextInputField TextInput { get; init; }

        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    TextInput.Value = value;
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
