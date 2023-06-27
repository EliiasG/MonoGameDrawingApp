using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MonoGameDrawingApp
{
    public class IOHelper
    {
        public static string RemoveExtention(string path)
        {
            return Path.ChangeExtension(path, "×").Replace(".×", "");
        }

        public static void OpenInExplorer(string path)
        {
            OpenWithDefaultProgram(Directory.Exists(path) ? path : Path.GetDirectoryName(path));
        }

        public static void OpenWithDefaultProgram(string path)
        {
            Process.Start("explorer.exe", path);
        }

        public static void CopyDirectory(string sourceDir, string destinationDir)
        {
            CopyDirectory(sourceDir, destinationDir, new HashSet<string>());
        }

        //Based on C# Docs, https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        private static void CopyDirectory(string sourceDir, string destinationDir, ISet<string> createdDirectories)
        {
            // Get information about the source directory
            DirectoryInfo dir = new(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
            }

            if (createdDirectories.Contains(sourceDir))
            {
                //If a directory is copied into itself, then we do not want to continue forever
                return;
            }

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);
            createdDirectories.Add(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, createdDirectories);
            }

        }
    }
}
