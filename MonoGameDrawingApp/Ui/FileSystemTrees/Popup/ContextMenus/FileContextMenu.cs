using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoGameDrawingApp.Ui.FileSystemTrees.Popup.ContextMenus
{
    public class FileContextMenu : IUiElement
    {
        private readonly IUiElement _root;


        public FileContextMenu(UiEnvironment environment, string path, PopupEnvironment popupEnvironment, FileTypeManager fileOpener)
        {
            Path = path;
            Environment = environment;
            PopupEnvironment = popupEnvironment;

            _root = new ContextMenu(environment, new List<IUiElement>
            {
                new ContextMenuButton(environment, "Open", Open),
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Rename", Rename),
                new ContextMenuButton(environment, "Delete", Delete),
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Cut", Cut),
                new ContextMenuButton(environment, "Copy", Copy),
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Show in explorer", OpenExplorer),
            }, popupEnvironment);

            FileOpener = fileOpener;
        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; }

        public string Path { get; }

        public PopupEnvironment PopupEnvironment { get; }

        public FileTypeManager FileOpener { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }

        private void Open()
        {
            PopupEnvironment.Close();
            FileOpener.OpenFile(Path);
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
                        Directory.Move(Path, string.Concat(Path.AsSpan(0, Math.Max(Path.LastIndexOf("/", StringComparison.Ordinal), Path.LastIndexOf("\\", StringComparison.Ordinal)) + 1), newName));
                        return;
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
                new ChoicePopupOption("Confirm", () => File.Delete(Path)),
            });

            PopupEnvironment.OpenCentered(choicePopup);
        }

        private void OpenExplorer()
        {
            PopupEnvironment.Close();
            IOHelper.OpenInExplorer(Path);
        }
    }
}
