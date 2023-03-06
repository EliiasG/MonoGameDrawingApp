using MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Actions;

namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo
{
    public class UndoRedoVectorSpiteAttachment : IVectorSpriteAttachment
    {
        private const int ActionAmount = 256;

        private int _index;
        private readonly IUndoRedoAction[] _actions;
        private IUndoRedoAction _current;
        private bool _disabled;

        public UndoRedoVectorSpiteAttachment()
        {
            _actions = new IUndoRedoAction[ActionAmount];
            _index = 0;
        }

        public void Attach(VectorSpriteItem item)
        {
            item.AddAttachment(new UndoRedoVectorSpriteItemAttachment(item, this));
        }

        public void Undo()
        {
            IUndoRedoAction action = Util.GetItemCircled(_actions, _index);
            if (action != null)
            {
                _disabled = true;

                action.Undo();
                --_index;

                _disabled = false;
            }
        }

        public void Redo()
        {
            IUndoRedoAction action = Util.GetItemCircled(_actions, _index + 1);
            if (action != null)
            {
                _disabled = true;

                action.Redo();
                ++_index;

                _disabled = false;
            }
        }

        internal void _dataChanging(VectorSpriteItem item)
        {
            if (!_disabled)
            {
                _add(new DataUndoRedoAction(item));
                _disabled = true;
            }
        }

        internal void _dataChanged(VectorSpriteItem item)
        {
            _current?.Done();
            _current = null;
        }

        internal void _childrenChanging(VectorSpriteItem item)
        {
            if (!_disabled)
            {
                _add(new ChildrenUndoRedoAction(item));
                _disabled = true;
            }
        }

        internal void _childrenChanged(VectorSpriteItem item)
        {
            _current?.Done();
            _current = null;
        }

        private void _add(IUndoRedoAction action)
        {
            _current = action;
            Util.SetItemCircled(_actions, ++_index, _current);
            Util.SetItemCircled(_actions, _index + 1, null);
        }

        public void ReAllow()
        {
            _disabled = false;
        }
    }
}
