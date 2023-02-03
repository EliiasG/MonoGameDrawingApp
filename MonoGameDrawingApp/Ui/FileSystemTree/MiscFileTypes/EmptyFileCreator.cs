using System.IO;

namespace MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes
{
    public class EmptyFileCreator : IFileCreator
    {
        public void CreateFile(string path)
        {
            File.Create(path).Close();
        }
    }
}
