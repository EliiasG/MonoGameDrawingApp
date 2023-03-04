namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Actions
{
    public interface IUndoRedoAction
    {
        void Undo();

        void Redo();

        void Done();
    }
}
