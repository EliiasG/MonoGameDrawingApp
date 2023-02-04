namespace MonoGameDrawingApp.Ui.FileSystemTrees
{
    public class CreatableFileType
    {
        public readonly string Name;
        public readonly string Extension;
        public readonly IFileCreator Creator;

        public CreatableFileType(IFileCreator fileCreator, string name, string extensionWithDot)
        {
            Name = name;
            Extension = extensionWithDot;
            Creator = fileCreator;
        }
    }
}
