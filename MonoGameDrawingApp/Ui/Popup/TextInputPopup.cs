using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Popup
{
    public class TextInputPopup : IUiElement
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
    }
}
