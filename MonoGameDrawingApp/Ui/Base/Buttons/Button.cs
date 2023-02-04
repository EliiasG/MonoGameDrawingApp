using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDrawingApp.Ui.Base.Buttons
{
    public class Button : IUiElement
    {
        public readonly IUiElement Child;

        private readonly UiEnvironment _environment;
        private Rectangle _bounds = Rectangle.Empty;
        private MouseState _oldMouse;

        public Button(UiEnvironment environment, IUiElement child)
        {
            _environment = environment;
            Child = child;
        }

        public bool Changed => Child.Changed;

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public bool ContainsMouse => _bounds.Contains(Mouse.GetState().Position);

        public bool LeftClicked => ContainsMouse && Mouse.GetState().LeftButton == ButtonState.Pressed;

        public bool RightClicked => ContainsMouse && Mouse.GetState().RightButton == ButtonState.Pressed;

        public bool JustLeftClicked => LeftClicked && !(_oldMouse.LeftButton == ButtonState.Pressed);

        public bool JustRightClicked => RightClicked && !(_oldMouse.RightButton == ButtonState.Pressed);

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return Child.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, width, height);
            _bounds = new Rectangle(position.ToPoint(), new Point(width, height));
            _oldMouse = Mouse.GetState();
        }
    }
}
