namespace MonoGameDrawingApp.Ui.FileSystemTrees.MiscFileTypes
{
    internal class DefaultExtensionIconCollection : IExtensionIconCollection
    {
        public ExtensionIcon[] ExtensionIcons => new ExtensionIcon[]
        {
            new ExtensionIcon("txt", "icons/textfile"),
            new ExtensionIcon("png", "icons/imagefile"),
            new ExtensionIcon("jpg", "icons/imagefile"),
            new ExtensionIcon("jpeg", "icons/imagefile"),
            new ExtensionIcon("jfif", "icons/imagefile"),
            new ExtensionIcon("pjpeg", "icons/imagefile"),
            new ExtensionIcon("pjp", "icons/imagefile"),
        };
    }
}
