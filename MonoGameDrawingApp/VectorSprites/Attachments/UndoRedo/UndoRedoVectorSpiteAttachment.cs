using MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Actions;

namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo
{
    public class UndoRedoVectorSpiteAttachment : IVectorSpriteAttachment
    {
        private const int ActionAmount = 256;

        private int _index;
        private readonly IUndoRedoAction[] _actions;
        private IUndoRedoAction _current;
        private bool _doing;

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
                _doing = true;

                action.Undo();
                --_index;

                _doing = false;
            }
        }

        public void Redo()
        {
            IUndoRedoAction action = Util.GetItemCircled(_actions, _index + 1);
            if (action != null)
            {
                _doing = true;

                action.Redo();
                ++_index;

                _doing = false;
            }
        }

        internal void _dataChanging(VectorSpriteItem item)
        {
            if (!_doing)
            {
                _add(new DataUndoRedoAction(item));
            }
        }

        internal void _dataChanged(VectorSpriteItem item)
        {
            _current?.Done();
            _current = null;
        }

        internal void _childrenChanging(VectorSpriteItem item)
        {
            if (!_doing)
            {
                _add(new ChildrenUndoRedoAction(item));
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
    }
}
