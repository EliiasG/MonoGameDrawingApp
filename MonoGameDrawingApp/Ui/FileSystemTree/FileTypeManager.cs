using MonoGameDrawingApp.Ui.Tabs;
using System.Collections.Generic;
using System.IO;

namespace MonoGameDrawingApp.Ui.FileSystemTree
{
    public class FileTypeManager
    {
        public readonly TabEnvironment TabEnvironment;
        public readonly Dictionary<string, IOpenableFileType> FileTypes;
        public readonly Dictionary<string, string> FileIcons;
        public readonly bool OpenUnknownFiles;

        private const string DefaultIconPath = "icons/file";

        public FileTypeManager(TabEnvironment tabEnvironment, IOpenableFileType[] fileTypes, FileIcon[] fileIcons, bool openUnknownFiles)
        {
            TabEnvironment = tabEnvironment;
            FileTypes = new Dictionary<string, IOpenableFileType>();
            FileIcons = new Dictionary<string, string>();
            OpenUnknownFiles = openUnknownFiles;

            foreach (IOpenableFileType fileType in fileTypes)
            {
                FileTypes.Add(fileType.Extention, fileType);
            }

            foreach (FileIcon fileIcon in fileIcons)
            {
                FileIcons.Add(fileIcon.Extention, fileIcon.IconPath);
            }

        }

        private string _fixedExtention(string path) => Path.GetExtension(path).Replace(".", "");

        public void OpenFile(string path)
        {
            string extention = _fixedExtention(path);

            if (FileTypes.ContainsKey(extention))
            {
                FileTypes[extention].Open(path, TabEnvironment);
            }
            else if (OpenUnknownFiles)
            {
                IOHelper.OpenWithDefaultProgram(path);
            }
        }

        public string GetIconPath(string path)
        {
            string extention = _fixedExtention(path);
            return FileIcons.ContainsKey(extention) ? FileIcons[extention] : DefaultIconPath;
        }
    }
}
