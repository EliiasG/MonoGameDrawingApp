using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui
{
    public interface IUiElement
    {

        bool Changed { get; }

        int RequiredWidth { get; }

        int RequiredHeight { get; }

        UiEnvironment Environment { get; }

        Texture2D Render(Graphics graphics, int width, int height);

        void Update(Vector2 position, int width, int height);
    }
}
