using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties
{
    public class IntModifierParameterView : IModifierParameterView
    {
        private IntInspectorProperty _property;

        public IntModifierParameterView(GeometryModifierParameter<int> parameter)
        {
            Parameter = parameter;
        }

        public GeometryModifierParameter<int> Parameter { get; init; }

        public IUiElement GenerateElement(UiEnvironment environment)
        {

            _property = new(environment, Parameter.Name + ": ", Parameter.Value, null);
            _property.ValueChanged += () =>
            {
                Parameter.Value = _property.Value;
                _property.Value = Parameter.Value;
            };
            Parameter.Changed += UpdateProperty;
            return _property;
        }

        public void Done()
        {
            Parameter.Changed -= UpdateProperty;
        }

        private void UpdateProperty()
        {
            _property.Value = Parameter.Value;
        }
    }
}
