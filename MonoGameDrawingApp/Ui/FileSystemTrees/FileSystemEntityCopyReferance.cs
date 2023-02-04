using System;
using System.IO;

namespace MonoGameDrawingApp.Ui.FileSystemTrees
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

        public void Paste(string path)
        {
            //Does not take care of exeptions, caller must do that
            path = System.IO.Path.Combine(path, System.IO.Path.GetFileName(Path)); //input path is directory to paste to, we want the path to the new entity (directory/file)

            //if the path exists, change the name
            if (Directory.Exists(path) || File.Exists(path))
            {
                string extention = System.IO.Path.GetExtension(path);
                string copyPath = IOHelper.RemoveExtention(path) + " - Copy";
                path = copyPath + extention;
                int i = 0;
                while (Directory.Exists(path) || File.Exists(path))
                {
                    path = copyPath + " (" + ++i + ")" + extention;
                }
            }

            if (Directory.Exists(Path))
            {
                _pasteDirectory(path);
            }
            else if (File.Exists(Path))
            {
                _pasteFile(path);
            }
            else
            {
                throw new FileNotFoundException("No file or directory '" + Path + "' found.");
            }
        }

        private void _pasteDirectory(string pastePath)
        {
            Action<string, string> paste = IsMoving ? Directory.Move : IOHelper.CopyDirectory;

            paste(Path, pastePath);
        }

        private void _pasteFile(string pastePath)
        {
            Action<string, string> paste = IsMoving ? File.Move : File.Copy;

            paste(Path, pastePath);
        }
    }
}
