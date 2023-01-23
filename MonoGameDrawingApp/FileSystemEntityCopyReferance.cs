namespace MonoGameDrawingApp
{
    public readonly struct FileSystemEntityCopyReferance
    {
        public FileSystemEntityCopyReferance(string path, bool isMoving)
        {
            Path = path;
            IsMoving = isMoving;
        }

        public bool IsMoving { get; init; }

        public string Path { get; init; }
    }
}
