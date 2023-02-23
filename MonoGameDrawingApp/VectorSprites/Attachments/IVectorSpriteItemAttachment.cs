using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.VectorSprites.Attachments
{
    public interface IVectorSpriteAttachment
    {
        void ChildrenChanged() { }

        void DataChanged() { }

        void ChildrenChanging() { }

        void DataChanging() { }
    }
}
