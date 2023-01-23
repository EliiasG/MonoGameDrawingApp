﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Buttons;
using MonoGameDrawingApp.Ui.Lists;
using MonoGameDrawingApp.Ui.Popup.ContextMenu.Items;
using MonoGameDrawingApp.Ui.TextInput.Filters;
using MonoGameDrawingApp.Ui.TextInput.Filters.Base;
using MonoGameDrawingApp.Ui.Tree.TreeItems.FileSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MonoGameDrawingApp.Ui.Popup.ContextMenu.Menus.FileSystem
{
    public class DirectoryContextMenu : IUiElement
    {
        public readonly string Path;
        public readonly PopupEnvironment PopupEnvironment;

        private readonly UiEnvironment _environment;

        private MouseState _oldMouse;

        private readonly StackView _outer;
        private readonly IUiElement _inner;
        private readonly Button _button;

        public DirectoryContextMenu(UiEnvironment environment, string path, PopupEnvironment popupEnvironment, bool isRoot)
        {
            Path = path;
            _environment = environment;
            PopupEnvironment = popupEnvironment;

            ContextMenuButton rename = new ContextMenuButton(environment, "Rename", _rename);
            ContextMenuButton delete = new ContextMenuButton(environment, "Delete", _delete);
            ContextMenuButton cut = new ContextMenuButton(environment, "Cut", _cut);
            ContextMenuButton copy = new ContextMenuButton(environment, "Copy", _copy);
            ContextMenuButton paste = new ContextMenuButton(environment, "Paste", () => throw new NotImplementedException());

            rename.Disabled = isRoot;
            delete.Disabled = isRoot;
            cut.Disabled = isRoot;
            copy.Disabled = isRoot;

            paste.Disabled = !(environment.Clipboard is FileSystemEntityCopyReferance);

            _inner = new VListView<IUiElement>(environment, new List<IUiElement>
            {
                rename,
                delete,
                new ContextMenuSeperator(environment),
                cut,
                copy,
                paste,
                new ContextMenuSeperator(environment),
                new ContextMenuButton(environment, "Add Folder", () => throw new NotImplementedException()),
                new ContextMenuButton(environment, "Add File", () => throw new NotImplementedException()),
            });

            _button = new Button(environment, new CenterView(environment, _inner, true, true));

            _outer = new StackView(environment, new IUiElement[]
                {
                    new ColorRect(environment, environment.Theme.SecondaryMenuBackgroundColor),
                    _button,
                }
            );

            _oldMouse = Mouse.GetState();
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

        private void _rename()
        {
            string name = System.IO.Path.GetFileName(Path);

            TextInputPopup popup = new TextInputPopup(
                environment: Environment,
                popupEnvironment: PopupEnvironment,
                confirmed: (string newName) =>
                {
                    string message;
                    if(newName == name)
                    {
                        return;
                    }
                    try
                    {
                        Directory.Move(Path, Path.Substring(0, Math.Max(Path.LastIndexOf("/"), Path.LastIndexOf("\\")) + 1) + newName);
                        return;
                    }
                    catch(IOException e)
                    {
                        message = e.Message + " This might be because its open, if it's open try closing it";
                    }
                    catch(Exception e) 
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
    }
}
