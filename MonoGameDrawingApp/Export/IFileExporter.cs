namespace MonoGameDrawingApp.Export
{
    public interface IFileExporter
    {
        string InputExtention { get; }

        string OutputExtention { get; }

        void Export(string fromPath, string exportFilePath);
    }
}
