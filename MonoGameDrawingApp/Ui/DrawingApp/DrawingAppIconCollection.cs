using MonoGameDrawingApp.Ui.FileSystemTrees;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class DrawingAppIconCollection : IExtensionIconCollection
    {
        public ExtensionIcon[] ExtensionIcons => new ExtensionIcon[]
        {
            new ExtensionIcon("vecspr", "icons/sprite")
        };
    }
}
