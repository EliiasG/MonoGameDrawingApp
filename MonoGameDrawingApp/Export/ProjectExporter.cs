using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MonoGameDrawingApp.Export
{
    public class ProjectExporter
    {
        public ProjectExporter(string creationTimesPath, string profilesPath, string sourcePath)
        {
            CreationTimesPath = creationTimesPath;
            ProfilesPath = profilesPath;
            SourcePath = sourcePath;
        }

        public string CreationTimesPath { get; }

        public string ProfilesPath { get; }

        public string SourcePath { get; }

        public void Export(string outputPath)
        {
            if (!Directory.Exists(SourcePath))
            {
                throw new IOException("Source directory does not exist: '" + SourcePath + "'");
            }

            Dictionary<string, long> creationTimes = _getCreationTimes();

            IFileExporter[] exporters = _getExporters();

            foreach (string path in Directory.EnumerateFiles(SourcePath))
            {
                long time = File.GetCreationTime(path).Ticks;
                if (creationTimes.ContainsKey(path) && creationTimes[path] == time)
                {
                    continue;
                }

                foreach (IFileExporter exporter in exporters)
                {
                    if (exporter.Extention != Path.GetExtension(path).Replace(".", ""))
                    {
                        continue;
                    }

                    string realativeDirectoryPath = Path.GetRelativePath(SourcePath, Path.GetDirectoryName(path));
                    string exportPath = Path.Combine(outputPath, realativeDirectoryPath);

                    Directory.CreateDirectory(exportPath);

                    exporter.Export(path, exportPath);
                }

                creationTimes[path] = time;
            }

            string[] lines = new string[creationTimes.Count * 2];

            int i = 0;
            foreach (string key in creationTimes.Keys)
            {
                lines[i++] = key;
                lines[i++] = creationTimes[key].ToString();
            }

            File.WriteAllLines(CreationTimesPath, lines);
        }

        private Dictionary<string, long> _getCreationTimes()
        {
            if (!File.Exists(CreationTimesPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CreationTimesPath));
                File.Create(CreationTimesPath).Close();
            }

            Dictionary<string, long> times = new();

            string name = "";

            foreach (string line in File.ReadLines(CreationTimesPath))
            {
                if (File.Exists(line))
                {
                    name = line;
                }
                else
                {
                    times.Add(name, long.Parse(line));
                }
            }

            return times;
        }

        private IFileExporter[] _getExporters()
        {
            if (!File.Exists(ProfilesPath))
            {
                throw new IOException("Missing profiles file: '" + ProfilesPath + "'");
            }
            using FileStream stream = File.OpenRead(ProfilesPath);

            object[][] objects = JsonSerializer.Deserialize<object[][]>(stream);

            IFileExporter[] exporters = new IFileExporter[objects.Length];

            for (int i = 0; i < objects.Length; i++)
            {
                object[] currentObjects = objects[i];
                if (currentObjects.Length < 1)
                {
                    throw new InvalidDataException("Cannot have empty export profile");
                }
                exporters[i] = currentObjects[0] switch
                {
                    string s => s switch
                    {
                        _ => throw new InvalidDataException("No such export profile: '" + s + "'"),
                    },
                    _ => throw new InvalidDataException("First element of profile mut be string")
                };
            }

            return exporters;
        }
    }
}
