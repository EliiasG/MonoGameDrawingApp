using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Descriptions;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteModifierInspectorView : IUiElement
    {
        public bool Changed => throw new NotImplementedException();

        public int RequiredWidth => throw new NotImplementedException();

        public int RequiredHeight => throw new NotImplementedException();

        public UiEnvironment Environment => throw new NotImplementedException();

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void Update(Vector2 position, int width, int height)
        {
            throw new NotImplementedException();
        }

        private static IModifierViewDescription _generateDescription(IVectorSpriteItemModifier modifier)
        {
            throw new NotImplementedException("No inspector implementation for: '" + modifier.GetType().Name + "'");
        }
    }
}
