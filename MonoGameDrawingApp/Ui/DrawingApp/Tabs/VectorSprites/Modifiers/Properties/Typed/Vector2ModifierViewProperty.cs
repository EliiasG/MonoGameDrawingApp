using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Properties;
using System;
using System.Numerics;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties.Typed
{
    public class Vector2ModifierViewProperty : ITypedModifierViewProperty<Vector2>
    {
        public Vector2ModifierViewProperty(string name, Vector2 value)
        {
            Name = name;
            Value = value;
        }

        public Action<Vector2> ValueChanged { get; set; }

        public string Name { get; init; }

        public Vector2 Value { get; set; }

        public IUiElement GenerateElement(UiEnvironment environment)
        {

            Vector2InspectorProperty property = new(environment, Name, Value, null);
            property.ValueChanged += () =>
            {
                ValueChanged?.Invoke(property.Value);
                Value = property.Value;
            };
            return property;
        }
    }
}
