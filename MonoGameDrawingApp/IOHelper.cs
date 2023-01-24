using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp
{
    public class IOHelper
    {
        public static void CopyDirectory(string sourceDir, string destinationDir)
        {
            _copyDirectory(sourceDir, destinationDir, new HashSet<string>());
        }

        //Based on C# Docs, https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        private static void _copyDirectory(string sourceDir, string destinationDir, ISet<string> createdDirectories)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

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
                _copyDirectory(subDir.FullName, newDestinationDir, createdDirectories);
            }
            
        }
    }
}
