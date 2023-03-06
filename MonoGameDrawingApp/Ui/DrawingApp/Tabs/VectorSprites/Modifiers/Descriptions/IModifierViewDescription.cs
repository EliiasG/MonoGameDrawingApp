using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Descriptions
{
    public interface IModifierViewDescription
    {
        public IEnumerable<IModifierViewProperty> Properties { get; }

        public string Name { get; }
    }
}