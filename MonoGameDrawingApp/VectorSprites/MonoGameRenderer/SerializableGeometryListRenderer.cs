using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.VectorSprites.Export;

namespace MonoGameDrawingApp.VectorSprites.MonoGameRenderer
{
    public class SerializableGeometryListRenderer
    {
        public void Render(SerializableGeometryList geometryList, GraphicsDevice graphicsDevice)
        {
            BasicEffect effect = new BasicEffect(graphicsDevice);
            effect.CurrentTechnique.Passes[0].Apply();
            //TODO continue here
        }
    }
}
