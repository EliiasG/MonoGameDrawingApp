using MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Actions;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo
{
    public class UndoRedoVectorSpiteAttachment : IVectorSpriteAttachment
    {
        private const int ActionAmount = 256;

        private int _index;
        private readonly IUndoRedoAction[] _actions;
        private readonly bool[] _skip;
        private readonly IList<IUndoRedoAction> _currentActions;
        private bool _disabled;

        public UndoRedoVectorSpiteAttachment()
        {
            _actions = new IUndoRedoAction[ActionAmount];
            _skip = new bool[ActionAmount];
            _currentActions = new List<IUndoRedoAction>();
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
            if (Util.GetItemCircled(_skip, _index + 1))
            {
                Undo();
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
            if (Util.GetItemCircled(_skip, _index))
            {
                Redo();
            }
        }

        internal void _dataChanging(VectorSpriteItem item)
        {
            _add(new DataUndoRedoAction(item));
        }

        internal void _childrenChanging(VectorSpriteItem item)
        {
            _add(new ChildrenUndoRedoAction(item));
        }

        private void _add(IUndoRedoAction action)
        {
            if (!_disabled && _currentActions.All((IUndoRedoAction a) => a.GetType() != action.GetType()))
            {
                _currentActions.Add(action);
            }
            /*
            Util.SetItemCircled(_actions, ++_index, _current);
            Util.SetItemCircled(_actions, _index + 1, null);
            */
        }

        public void Tick()
        {
            bool skip = false;
            foreach (IUndoRedoAction action in _currentActions)
            {
                action.Done();
                Util.SetItemCircled(_actions, ++_index, action);
                Util.SetItemCircled(_actions, _index + 1, null);
                Util.SetItemCircled(_skip, _index, skip);
                skip = true;
            }
            _currentActions.Clear();
        }
    }
}
