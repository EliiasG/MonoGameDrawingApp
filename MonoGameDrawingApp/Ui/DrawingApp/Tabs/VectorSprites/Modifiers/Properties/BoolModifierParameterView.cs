using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties.Typed
{
    public class BoolModifierParameterView : IModifierParameterView
    {
        private BoolInspectorProperty _property;

        public BoolModifierParameterView(GeometryModifierParameter<bool> parameter)
        {
            Parameter = parameter;
        }

        public GeometryModifierParameter<bool> Parameter { get; init; }

        public IUiElement GenerateElement(UiEnvironment environment)
        {

            _property = new(environment, Parameter.Name + ": ", Parameter.Value, null);
            _property.ValueChanged += () =>
            {
                Parameter.Value = _property.Value;
                _property.Value = Parameter.Value;
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
