using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties.Typed;
using MonoGameDrawingApp.VectorSprites.Modifiers.Applyable.Simple;
using System.Collections.Generic;
using System.Numerics;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Descriptions
{
    public class MoveModifierViewDescription : IModifierViewDescription
    {
        public MoveModifierViewDescription(MoveModifier modifier)
        {
            Modifier = modifier;
        }

        public MoveModifier Modifier { get; init; }

        public string Name => "Move";

        public IEnumerable<IModifierViewProperty> GenerateProperties()
        {
            return new List<IModifierViewProperty>
            {
                new Vector2ModifierViewProperty("Offset: ", Modifier.Offset)
                {
                    ValueChanged = (Vector2 v) => Modifier.Offset = v,
                },
            };
        }
    }
}
