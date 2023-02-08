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
        public readonly string Path;
        public readonly PopupEnvironment PopupEnvironment;
        public readonly FileTypeManager FileTypeManager;

        private readonly UiEnvironment _environment;

        private readonly IUiElement _root;

        public DirectoryContextMenu(UiEnvironment environment, string path, PopupEnvironment popupEnvironment, bool isRoot, FileTypeManager fileTypeManager)
        {
            Path = path;
            _environment = environment;
            PopupEnvironment = popupEnvironment;
            FileTypeManager = fileTypeManager;

            ContextMenuButton rename = new ContextMenuButton(environment, "Rename", _rename);
            ContextMenuButton delete = new ContextMenuButton(environment, "Delete", _delete);
            ContextMenuButton cut = new ContextMenuButton(environment, "Cut", _cut);
            ContextMenuButton copy = new ContextMenuButton(environment, "Copy", _copy);
            ContextMenuButton paste = new ContextMenuButton(environment, "Paste", _paste);

            rename.Disabled = isRoot;
            delete.Disabled = isRoot;
            cut.Disabled = isRoot;
            copy.Disabled = isRoot;

            paste.Disabled = !(environment.Clipboard is FileSystemEntityCopyReferance);

            _root = new ContextMenu(Environment, new IUiElement[]
            {
                rename,
                delete,
                new ContextMenuSeperator(environment),
                cut,
                copy,
                paste,
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Add Directory", _addDirectory),
                new ContextMenuButton(environment, "Add File", _addFile),
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Show in explorer", _openExplorer),
            }, popupEnvironment);
        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }

        private void _rename()
        {
            string name = System.IO.Path.GetFileName(Path);

            TextInputPopup popup = new TextInputPopup(
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
                        Directory.Move(Path, Path.Substring(0, Math.Max(Path.LastIndexOf("/"), Path.LastIndexOf("\\")) + 1) + newName);
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
                    MessagePopup messagePopup = new MessagePopup(Environment, message, PopupEnvironment);
                    PopupEnvironment.OpenCentered(messagePopup);
                },
                filters: new ITextInputFilter[] { new FileSystemTextInputFilter() },
                title: "Rename: '" + name + "'",
                currentValue: name
            );

            PopupEnvironment.OpenCentered(popup);
        }

        private void _copy()
        {
            Environment.Clipboard = new FileSystemEntityCopyReferance(Path, false);
            PopupEnvironment.Close();
        }

        private void _cut()
        {
            Environment.Clipboard = new FileSystemEntityCopyReferance(Path, true);
            PopupEnvironment.Close();
        }

        private void _delete()
        {
            ChoicePopup choicePopup = new ChoicePopup(Environment, "Delete '" + System.IO.Path.GetFileName(Path) + "'?", PopupEnvironment, new ChoicePopupOption[]
            {
                new ChoicePopupOption("Cancel", () => {}),
                new ChoicePopupOption("Confirm", () => Directory.Delete(Path, true)),
            });

            PopupEnvironment.OpenCentered(choicePopup);
        }

        private void _paste()
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
                    MessagePopup messagePopup = new MessagePopup(Environment, e.Message, PopupEnvironment);
                    PopupEnvironment.OpenCentered(messagePopup);
                }
            }
            else
            {
                MessagePopup messagePopup = new MessagePopup(Environment, "Clipboard is not file / directory", PopupEnvironment);
                PopupEnvironment.OpenCentered(messagePopup);
            }
        }

        private void _openExplorer()
        {
            PopupEnvironment.Close();
            IOHelper.OpenInExplorer(Path);
        }

        private void _addDirectory()
        {
            TextInputPopup popup = new TextInputPopup(
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
                        MessagePopup messagePopup = new MessagePopup(Environment, e.Message, PopupEnvironment);
                        PopupEnvironment.OpenCentered(messagePopup);
                    }
                },
                filters: new ITextInputFilter[] { new FileSystemTextInputFilter() },
                title: "Add Folder:"
            );

            PopupEnvironment.OpenCentered(popup);
        }

        private void _addFile()
        {
            PopupEnvironment.OpenCentered(new FileCreatorMenu(Environment, FileTypeManager, PopupEnvironment, Path));
        }
    }
}
