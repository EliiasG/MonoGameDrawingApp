using MonoGameDrawingApp.Ui.Base;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties
{
    public interface IInspectorProperty<T> : IUiElement
    {
        public T Value { get; set; }

        public Action ValueChanged { get; set; }
    }
}
