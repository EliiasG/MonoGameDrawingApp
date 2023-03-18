using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties
{
    public class VectorSpriteItemReferenceModifierParameterView : IModifierParameterView
    {
        private VectorSpriteItemInspectorProperty _property;

        public VectorSpriteItemReferenceModifierParameterView(GeometryModifierParameter<VectorSpriteItemReference> parameter, VectorSpriteTabView vectorSpriteTabView)
        {
            Parameter = parameter;
            VectorSpriteTabView = vectorSpriteTabView;
        }

        public VectorSpriteTabView VectorSpriteTabView { get; }

        public GeometryModifierParameter<VectorSpriteItemReference> Parameter { get; init; }

        public IUiElement GenerateElement(UiEnvironment environment)
        {

            _property = new(environment, Parameter.Name + ": ", Parameter.Value.Item, VectorSpriteTabView, null);
            _property.ValueChanged += () =>
            {
                Parameter.Value.Item = _property.Value;
                _property.Value = Parameter.Value.Item;
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
            _property.Value = Parameter.Value.Item;
        }
    }
}
