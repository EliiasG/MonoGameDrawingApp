using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public interface IUiElement
    {
        Texture2D Render(Graphics graphics, Vector2 position, int width, int height);
        int RequiredWidth { get; }
        int RequiredHeight { get; }
    }
}
