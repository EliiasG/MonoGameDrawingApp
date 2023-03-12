using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties.Typed
{
    public class FloatModifierParameterView : IModifierParameterView
    {
        private FloatInspectorProperty _property;

        public FloatModifierParameterView(GeometryModifierParameter<float> parameter)
        {
            Parameter = parameter;
        }

        public GeometryModifierParameter<float> Parameter { get; init; }

        public IUiElement GenerateElement(UiEnvironment environment)
        {

            _property = new(environment, Parameter.Name + ": ", Parameter.Value, null);
            _property.ValueChanged += () =>
            {
                Parameter.Value = _property.Value;
            };
            Parameter.Changed += _updateProperty;
            return _property;
        }

        public void Done()
        {
            Parameter.Changed -= _updateProperty;
        }

        private void _updateProperty()
        {
            _property.Value = Parameter.Value;
        }
    }
}
