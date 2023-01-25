using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Scroll
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
