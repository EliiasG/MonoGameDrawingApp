using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Drawing;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties
{
    public class ColorModifierParameterView : IModifierParameterView
    {
        private ColorInspectorProperty _property;

        public ColorModifierParameterView(GeometryModifierParameter<Color> parameter, PopupEnvironment popupEnvironment)
        {
            Parameter = parameter;
            PopupEnvironment = popupEnvironment;
        }

        public PopupEnvironment PopupEnvironment { get; }

        public GeometryModifierParameter<Color> Parameter { get; init; }

        public IUiElement GenerateElement(UiEnvironment environment)
        {

            _property = new(environment, PopupEnvironment, Parameter.Name + ": ", Parameter.Value, null);
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
