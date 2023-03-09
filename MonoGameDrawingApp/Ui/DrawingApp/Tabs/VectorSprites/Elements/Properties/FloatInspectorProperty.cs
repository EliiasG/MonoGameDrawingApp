using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Types;
using System;
using System.Globalization;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Properties
{
    public class FloatInspectorProperty : IInspectorProperty<float>
    {
        private readonly StringInspectorProperty _stringInspectorProperty;

        private float _value;

        public FloatInspectorProperty(UiEnvironment environment, string name, float value, Action changed)
        {
            Environment = environment;

            ValueChanged = changed;

            _stringInspectorProperty = new StringInspectorProperty(environment, name, value.ToString(), new ITextInputFilter[] { new FloatTextInputFilter() }, () =>
            {
                try
                {
                    Value = float.Parse(_stringInspectorProperty.Value, CultureInfo.InvariantCulture);
                    ValueChanged?.Invoke();
                }
                catch
                {
                    _stringInspectorProperty.Value = Value.ToString(CultureInfo.InvariantCulture);
                }
            }, 75);

        }

        public float Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _stringInspectorProperty.Value = value.ToString(CultureInfo.InvariantCulture);
                    _value = value;
                }
            }
        }
        public Action ValueChanged { get; set; }

        public bool Changed => _stringInspectorProperty.Changed;

        public int RequiredWidth => _stringInspectorProperty.RequiredWidth;

        public int RequiredHeight => _stringInspectorProperty.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _stringInspectorProperty.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _stringInspectorProperty.Update(position, width, height);
        }
    }
}
