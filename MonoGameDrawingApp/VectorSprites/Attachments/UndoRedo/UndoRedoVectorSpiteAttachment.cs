using MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Actions;
using System.Collections.Generic;

namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo
{
    public class UndoRedoVectorSpiteAttachment : IVectorSpriteAttachment
    {
        private const int ActionAmount = 512;

        private int _index;
        private readonly IUndoRedoAction[] _actions;
        private readonly bool[] _skip;
        private readonly Dictionary<VectorSpriteItem, ISet<IUndoRedoAction>> _currentActions;
        private bool _disabled;

        public UndoRedoVectorSpiteAttachment()
        {
            _actions = new IUndoRedoAction[ActionAmount];
            _skip = new bool[ActionAmount];
            _currentActions = new Dictionary<VectorSpriteItem, ISet<IUndoRedoAction>>();
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

        internal void DataChanging(VectorSpriteItem item)
        {
            Add(new DataUndoRedoAction(item), item);
        }

        internal void ChildrenChanging(VectorSpriteItem item)
        {
            Add(new ChildrenUndoRedoAction(item), item);
        }

        private void Add(IUndoRedoAction action, VectorSpriteItem item)
        {
            if (!_currentActions.ContainsKey(item))
            {
                _currentActions[item] = new HashSet<IUndoRedoAction>();
            }

            if (!_disabled && !_currentActions[item].Contains(action))
            {
                _currentActions[item].Add(action);
            }
        }

        public void Tick()
        {
            bool skip = false;
            foreach (ISet<IUndoRedoAction> actions in _currentActions.Values)
            {
                foreach (IUndoRedoAction action in actions)
                {
                    action.Done();
                    Util.SetItemCircled(_actions, ++_index, action);
                    Util.SetItemCircled(_actions, _index + 1, null);
                    Util.SetItemCircled(_skip, _index, skip);
                    skip = true;
                }
            }
            _currentActions.Clear();
        }
    }
}
