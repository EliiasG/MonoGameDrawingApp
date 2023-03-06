using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Modifiers.Properties
{
    public interface ITypedModifierViewProperty<T> : IModifierViewProperty
    {
        public Action<T> ValueChanged { get; set;}
    }
}
