using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties
{
    public class Vector2InspectorProperty : IInspectorProperty<Vector2>
    {

        private readonly IUiElement _root;

        private readonly FloatInspectorProperty _xProperty;
        private readonly FloatInspectorProperty _yProperty;

        private Vector2 _value;

        public Vector2InspectorProperty(UiEnvironment environment, string name, Vector2 value, Action changed)
        {
            Environment = environment;

            ValueChanged = changed;

            _xProperty = new FloatInspectorProperty(Environment, "X: ", value.X, () =>
            {
                Value = new Vector2(_xProperty.Value, Value.Y);
                ValueChanged?.Invoke();
            });

            _yProperty = new FloatInspectorProperty(Environment, "Y: ", value.Y, () =>
            {
                Value = new Vector2(Value.X, _yProperty.Value);
                ValueChanged?.Invoke();
            });

            _root = new NamedInspectorProperty(
                environment: Environment,
                child: new VListView<IUiElement>(
                    environment: Environment,
                    items: new List<IUiElement>
                    {
                        _xProperty,
                        _yProperty,
                    }
                ),
                text: name
            );
        }

        public Vector2 Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    _xProperty.Value = value.X;
                    _yProperty.Value = value.Y;
                }
            }
        }

        public Action ValueChanged { get; set; }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Microsoft.Xna.Framework.Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }
    }
}
