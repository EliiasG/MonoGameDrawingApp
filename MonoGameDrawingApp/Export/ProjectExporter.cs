using MonoGameDrawingApp.Export.VectorSprites;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MonoGameDrawingApp.Export
{
    public class ProjectExporter
    {
        public ProjectExporter(string profilesPath, string sourcePath, Graphics graphics)
        {
            ProfilesPath = profilesPath;
            SourcePath = sourcePath;
            Graphics = graphics;
        }

        public string ProfilesPath { get; }

        public string SourcePath { get; }

        public Graphics Graphics { get; }

        public void Export(string outputPath)
        {
            if (!Directory.Exists(SourcePath))
            {
                throw new IOException("Source directory does not exist: '" + SourcePath + "'");
            }

            (IFileExporter, string)[] exporters = _getExporters();

            Directory.CreateDirectory(outputPath);

            ISet<string> exportedFiles = new HashSet<string>();

            foreach (string path in Directory.EnumerateFiles(outputPath, "", SearchOption.AllDirectories))
            {
                exportedFiles.Add(path);
            }


            foreach (string path in Directory.EnumerateFiles(SourcePath, "", SearchOption.AllDirectories))
            {
                long time = File.GetLastWriteTimeUtc(path).Ticks;

                string realativeDirectoryPath = Path.GetRelativePath(SourcePath, Path.GetDirectoryName(path));
                string exportDirectoryPath = realativeDirectoryPath == "." ? outputPath : Path.Combine(outputPath, realativeDirectoryPath);

                foreach ((IFileExporter, string) exporter in exporters)
                {
                    if (exporter.Item1.InputExtention != Path.GetExtension(path).Replace(".", ""))
                    {
                        continue;
                    }

                    string exportedPath = Path.Combine(exportDirectoryPath, IOHelper.RemoveExtention(Path.GetFileName(path)) + exporter.Item2 + "." + exporter.Item1.OutputExtention);

                    Directory.CreateDirectory(exportDirectoryPath);

                    exportedFiles.Remove(exportedPath);

                    if (File.Exists(exportedPath))
                    {
                        if (File.GetLastWriteTimeUtc(exportedPath).Ticks > time)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        File.Create(exportedPath).Close();
                    }

                    exporter.Item1.Export(path, exportedPath);
                }
            }

            foreach (string path in exportedFiles)
            {
                File.Delete(path);
            }
        }

        private (IFileExporter, string)[] _getExporters()
        {
            if (!File.Exists(ProfilesPath))
            {
                throw new IOException("Missing profiles file: '" + ProfilesPath + "'");
            }
            using FileStream stream = File.OpenRead(ProfilesPath);

            JsonValue[][] objects = JsonSerializer.Deserialize<JsonValue[][]>(stream);

            (IFileExporter, string)[] exporters = new (IFileExporter, string)[objects.Length];

            for (int i = 0; i < objects.Length; i++)
            {
                JsonValue[] currentValues = objects[i];
                if (currentValues.Length < 2)
                {
                    throw new InvalidDataException("Export profiles must contain type and prefix");
                }

                string name = currentValues[0].Deserialize<string>();

                exporters[i] = (
                    name switch
                    {
                        "Png" => new VectorSpritePngExporter(currentValues[2].Deserialize<int>(), currentValues[3].Deserialize<bool>(), Graphics),
                        _ => throw new InvalidDataException("No such export profile: '" + name + "'"),
                    },
                    currentValues[1].Deserialize<string>()
                );

            }

            return exporters;
        }
    }
}
