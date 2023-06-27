using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using System;
using System.IO;

namespace MonoGameDrawingApp.Ui.FileSystemTrees.Popup.ContextMenus
{
    public class DirectoryContextMenu : IUiElement
    {
        public DirectoryContextMenu(UiEnvironment environment, string path, PopupEnvironment popupEnvironment, bool isRoot, FileTypeManager fileTypeManager)
        {
            Path = path;
            Environment = environment;
            PopupEnvironment = popupEnvironment;
            FileTypeManager = fileTypeManager;

            ContextMenuButton rename = new(environment, "Rename", Rename);
            ContextMenuButton delete = new(environment, "Delete", Delete);
            ContextMenuButton cut = new(environment, "Cut", Cut);
            ContextMenuButton copy = new(environment, "Copy", Copy);
            ContextMenuButton paste = new(environment, "Paste", Paste);

            rename.Disabled = isRoot;
            delete.Disabled = isRoot;
            cut.Disabled = isRoot;
            copy.Disabled = isRoot;

            paste.Disabled = environment.Clipboard is not FileSystemEntityCopyReferance;

            Root = new ContextMenu(Environment, new IUiElement[]
            {
                rename,
                delete,
                new ContextMenuSeperator(environment),
                cut,
                copy,
                paste,
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Add Directory", AddDirectory),
                new ContextMenuButton(environment, "Add File", AddFile),
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Show in explorer", OpenExplorer),
            }, popupEnvironment);
        }

        public bool Changed => Root.Changed;

        public int RequiredWidth => Root.RequiredWidth;

        public int RequiredHeight => Root.RequiredHeight;

        public UiEnvironment Environment { get; }

        public PopupEnvironment PopupEnvironment { get; }

        public string Path { get; }

        public FileTypeManager FileTypeManager { get; }

        public IUiElement Root { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return Root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            Root.Update(position, width, height);
        }

        private void Rename()
        {
            string name = System.IO.Path.GetFileName(Path);

            TextInputPopup popup = new(
                environment: Environment,
                popupEnvironment: PopupEnvironment,
                confirmed: (newName) =>
                {
                    string message;
                    if (newName == name)
                    {
                        return;
                    }
                    try
                    {
                        int lastSlashIndex = Math.Max(Path.LastIndexOf("/", StringComparison.Ordinal), Path.LastIndexOf("\\", StringComparison.Ordinal)) + 1;
                        Directory.Move(Path, string.Concat(Path.AsSpan(0, lastSlashIndex), newName));
                        return;
                    }
                    catch (IOException e)
                    {
                        message = e.Message + " This might be because its open, if it's open try closing it";
                    }
                    catch (Exception e)
                    {
                        message = e.Message;
                    }
                    MessagePopup messagePopup = new(Environment, message, PopupEnvironment);
                    PopupEnvironment.OpenCentered(messagePopup);
                },
                filters: new ITextInputFilter[] { new FileSystemTextInputFilter() },
                title: "Rename: '" + name + "'",
                currentValue: name
            );

            PopupEnvironment.OpenCentered(popup);
        }

        private void Copy()
        {
            Environment.Clipboard = new FileSystemEntityCopyReferance(Path, false);
            PopupEnvironment.Close();
        }

        private void Cut()
        {
            Environment.Clipboard = new FileSystemEntityCopyReferance(Path, true);
            PopupEnvironment.Close();
        }

        private void Delete()
        {
            ChoicePopup choicePopup = new(Environment, "Delete '" + System.IO.Path.GetFileName(Path) + "'?", PopupEnvironment, new ChoicePopupOption[]
            {
                new ChoicePopupOption("Cancel", () => {}),
                new ChoicePopupOption("Confirm", () => Directory.Delete(Path, true)),
            });

            PopupEnvironment.OpenCentered(choicePopup);
        }

        private void Paste()
        {
            PopupEnvironment.Close();
            if (Environment.Clipboard is FileSystemEntityCopyReferance clipboard)
            {
                try
                {
                    clipboard.Paste(Path);
                }
                catch (Exception e)
                {
                    MessagePopup messagePopup = new(Environment, e.Message, PopupEnvironment);
                    PopupEnvironment.OpenCentered(messagePopup);
                }
            }
            else
            {
                MessagePopup messagePopup = new(Environment, "Clipboard is not file / directory", PopupEnvironment);
                PopupEnvironment.OpenCentered(messagePopup);
            }
        }

        private void OpenExplorer()
        {
            PopupEnvironment.Close();
            IOHelper.OpenInExplorer(Path);
        }

        private void AddDirectory()
        {
            TextInputPopup popup = new(
                environment: Environment,
                popupEnvironment: PopupEnvironment,
                confirmed: (name) =>
                {
                    try
                    {
                        Directory.CreateDirectory(System.IO.Path.Combine(Path, name));
                    }
                    catch (Exception e)
                    {
                        MessagePopup messagePopup = new(Environment, e.Message, PopupEnvironment);
                        PopupEnvironment.OpenCentered(messagePopup);
                    }
                },
                filters: new ITextInputFilter[] { new FileSystemTextInputFilter() },
                title: "Add Directory:"
            );

            PopupEnvironment.OpenCentered(popup);
        }

        private void AddFile()
        {
            PopupEnvironment.OpenCentered(new FileCreatorMenu(Environment, FileTypeManager, PopupEnvironment, Path));
        }
    }
}
