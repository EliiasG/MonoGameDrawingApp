using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public interface IUiElement
    {
        Texture2D Render(Graphics graphics, int width, int height);

        void Update(Vector2 position, int width, int height);

        bool Changed { get; }

        int RequiredWidth { get; }

        int RequiredHeight { get; }

    }
}
