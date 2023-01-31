using MonoGameDrawingApp.Ui.Tabs;
using System.Collections.Generic;
using System.IO;

namespace MonoGameDrawingApp.Ui.FileSystemTree
{
    public class FileTypeManager
    {
        public readonly TabEnvironment TabEnvironment;
        public readonly Dictionary<string, IOpenableFileType> FileTypes;
        public readonly Dictionary<string, string> ExtentionIcons;
        public readonly CreatableFileType[] CreatableFileTypes;
        public readonly bool OpenUnknownFiles;

        private const string DefaultIconPath = "icons/file";

        public FileTypeManager(TabEnvironment tabEnvironment, IOpenableFileType[] fileTypes, IExtensionIconCollection extensionIconCollection, CreatableFileType[] creatableTypes, bool openUnknownFiles)
        {
            TabEnvironment = tabEnvironment;
            FileTypes = new Dictionary<string, IOpenableFileType>();
            ExtentionIcons = new Dictionary<string, string>();
            OpenUnknownFiles = openUnknownFiles;
            CreatableFileTypes = creatableTypes;

            foreach (IOpenableFileType fileType in fileTypes)
            {
                foreach (string extention in fileType.Extentions)
                {
                    FileTypes.Add(extention, fileType);
                }
            }

            foreach (ExtensionIcon fileIcon in extensionIconCollection.ExtensionIcons)
            {
                ExtentionIcons.Add(fileIcon.Extention, fileIcon.IconPath);
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
            return ExtentionIcons.ContainsKey(extention) ? ExtentionIcons[extention] : DefaultIconPath;
        }
    }
}
