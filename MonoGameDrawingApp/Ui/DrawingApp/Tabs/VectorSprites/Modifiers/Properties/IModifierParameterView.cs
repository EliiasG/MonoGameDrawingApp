using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties
{
    public interface IModifierParameterView
    {
        IUiElement GenerateElement(UiEnvironment environment);

        void Done();
    }
}
