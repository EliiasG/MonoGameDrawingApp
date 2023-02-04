using Microsoft.Xna.Framework;

namespace MonoGameDrawingApp.Ui.Base.Scroll
{
    public interface IScrollableView : IUiElement
    {
        public Vector2 Position { get; set; }
        public int Width { get; }
        public int Height { get; }
        public int MaxWidth { get; }
        public int MaxHeight { get; }
    }
}
