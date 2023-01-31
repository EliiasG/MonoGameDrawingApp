namespace MonoGameDrawingApp.Ui.FileSystemTree
{
    public class CreatableFileType
    {
        public readonly string Name;
        public readonly string Extention;
        public readonly IFileCreator Creator;

        public CreatableFileType(IFileCreator fileCreator, string name, string extentionWithDot)
        {
            Name = name;
            Extention = extention;
            Creator = fileCreator;
        }
    }
}
