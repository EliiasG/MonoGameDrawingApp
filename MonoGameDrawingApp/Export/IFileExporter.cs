namespace MonoGameDrawingApp.Export
{
    public interface IFileExporter
    {
        string Extention { get; }

        void Export(string fromPath, string exportDirPath);
    }
}
