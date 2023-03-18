using System;

namespace MonoGameDrawingApp
{
    public interface IListenable
    {
        public Action Changing { get; set; }

        public Action Changed { get; set; }
    }
}
