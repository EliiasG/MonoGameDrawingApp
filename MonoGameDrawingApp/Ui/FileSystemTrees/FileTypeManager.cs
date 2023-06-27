using MonoGameDrawingApp.Ui.Base.Tabs;
using System.Collections.Generic;
using System.IO;

namespace MonoGameDrawingApp.Ui.FileSystemTrees
{
    public class FileTypeManager
    {
        private const string DefaultIconPath = "icons/file";

        public TabEnvironment TabEnvironment { get; }

        public Dictionary<string, IOpenableFileType> FileTypes { get; }

        public Dictionary<string, string> ExtentionIcons { get; }

        public CreatableFileType[] CreatableFileTypes { get; }

        public bool OpenUnknownFiles { get; }

        public FileTypeManager(TabEnvironment tabEnvironment, IOpenableFileType[] fileTypes, IExtensionIconCollection[] extensionIconCollections, CreatableFileType[] creatableTypes, bool openUnknownFiles)
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
            foreach (IExtensionIconCollection extensionIconCollection in extensionIconCollections)
            {
                foreach (ExtensionIcon fileIcon in extensionIconCollection.ExtensionIcons)
                {
                    ExtentionIcons.Add(fileIcon.Extention, fileIcon.IconPath);
                }
            }

        }

        private static string FixedExtention(string path)
        {
            return Path.GetExtension(path).Replace(".", "");
        }

        public void OpenFile(string path)
        {
            string extention = FixedExtention(path);

            if (FileTypes.TryGetValue(extention, out IOpenableFileType value))
            {
                value.Open(path, TabEnvironment);
            }
            else if (OpenUnknownFiles)
            {
                IOHelper.OpenWithDefaultProgram(path);
            }
        }

        public string GetIconPath(string path)
        {
            string extention = FixedExtention(path);
            return ExtentionIcons.TryGetValue(extention, out string value) ? value : DefaultIconPath;
        }
    }
}
