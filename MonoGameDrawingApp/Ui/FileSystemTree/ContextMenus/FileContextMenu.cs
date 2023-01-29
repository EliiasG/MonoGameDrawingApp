﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenu.Items;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using MonoGameDrawingApp.Ui.Buttons;
using MonoGameDrawingApp.Ui.List;
using MonoGameDrawingApp.Ui.Popup;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoGameDrawingApp.Ui.FileSystemTree.ContextMenus
{
    public class FileContextMenu : IUiElement
    {
        public readonly string Path;
        public readonly PopupEnvironment PopupEnvironment;
        public readonly FileTypeManager FileOpener;

        private readonly UiEnvironment _environment;

        private MouseState _oldMouse;

        private readonly StackView _outer;
        private readonly IUiElement _inner;
        private readonly Button _button;


        public FileContextMenu(UiEnvironment environment, string path, PopupEnvironment popupEnvironment, FileTypeManager fileOpener)
        {
            Path = path;
            _environment = environment;
            PopupEnvironment = popupEnvironment;

            _inner = new VListView<IUiElement>(environment, new List<IUiElement>
            {
                new ContextMenuButton(environment, "Open", _open),
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Rename", _rename),
                new ContextMenuButton(environment, "Delete", _delete),
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Cut", _cut),
                new ContextMenuButton(environment, "Copy", _copy),
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Show in explorer", _openExplorer),
            });

            _button = new Button(environment, new CenterView(environment, _inner, true, true));

            _outer = new StackView(environment, new IUiElement[]
                {
                    new ColorRect(environment, environment.Theme.SecondaryMenuBackgroundColor),
                    _button,
                }
            );

            _oldMouse = Mouse.GetState();
            FileOpener = fileOpener;
        }

        public bool Changed => _outer.Changed;

        public int RequiredWidth => _inner.RequiredWidth + 8;

        public int RequiredHeight => _inner.RequiredHeight + 8;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            bool justPressedLeft = mouse.LeftButton == ButtonState.Pressed && _oldMouse.LeftButton != ButtonState.Pressed;
            bool justPressedRight = mouse.RightButton == ButtonState.Pressed && _oldMouse.RightButton != ButtonState.Pressed;
            if (!_button.ContainsMouse && (justPressedLeft || justPressedRight))
            {
                PopupEnvironment.Close();
            }
            _outer.Update(position, width, height);
            _oldMouse = mouse;
        }

        private void _open()
        {
            PopupEnvironment.Close();
            FileOpener.OpenFile(Path);
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
                new ChoicePopupOption("Confirm", () => File.Delete(Path)),
            });

            PopupEnvironment.OpenCentered(choicePopup);
        }

        private void _openExplorer()
        {
            PopupEnvironment.Close();
            IOHelper.OpenInExplorer(Path);
        }
    }
}
