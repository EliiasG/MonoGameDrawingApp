using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.TextInput;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.FileSystemTrees.MiscFileTypes;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.FileSystemTrees.Popup
{

    public class FileCreatorMenu : IUiElement
    {
        private const int Spacing = 5;
        private const int TextSpacing = 5;
        private const int ListWidth = 200;
        private const int ListHeight = 400;

        private readonly IUiElement _root;
        private readonly TextInputField _textInputField;
        private readonly List<ContextMenuButton> _buttons;

        private CreatableFileType _selected;


        public FileCreatorMenu(UiEnvironment environment, FileTypeManager fileTypeManager, PopupEnvironment popupEnvironment, string path)
        {
            Environment = environment;
            FileTypeManager = fileTypeManager;
            _selected = new CreatableFileType(new EmptyFileCreator(), "Other", "");
            _buttons = new List<ContextMenuButton>();
            PopupEnvironment = popupEnvironment;
            Path = path;

            List<IUiElement> typeButtons = new();

            foreach (CreatableFileType creatableFileType in fileTypeManager.CreatableFileTypes)
            {
                typeButtons.Add(GenerateButton(creatableFileType, false));
            }

            _textInputField = new TextInputField(Environment, _selected.Name, new ITextInputFilter[] { new FileSystemTextInputFilter() }, false, false, false, 100)
            {
                IsSelected = true,
                Extention = _selected.Extension
            };

            typeButtons.Add(GenerateButton(_selected, true));

            _root = new StackView(Environment, new IUiElement[]
            {
                new ColorRect(Environment, Environment.Theme.SecondaryMenuBackgroundColor),
                new VListView<IUiElement>(Environment, new List<IUiElement>
                {
                    new EmptySpace(Environment,  1, Spacing),
                    new HListView<IUiElement>(Environment, new List<IUiElement>()
                    {
                        new EmptySpace(Environment,  Spacing, 1),
                        new MinSize(Environment, new ScrollWindow(Environment, new VScrollableListView(Environment, typeButtons, false, 1)), ListWidth, ListHeight),
                        new MinSize(Environment, new ColorRect(Environment  , Color.Transparent), Spacing, 1),
                    }),
                    new EmptySpace(Environment,  1, Spacing),
                    new CenterView(Environment, new MinSize(Environment, _textInputField, ListWidth, (int) Environment.FontHeight + TextSpacing), true, false),
                    new EmptySpace(Environment,  1, Spacing),
                    new CenterView(Environment, new HListView<IUiElement>(Environment, new List<IUiElement>
                    {
                        new ContextMenuButton(Environment, "Cancel", PopupEnvironment.Close),
                        new EmptySpace(Environment,  Spacing, 1),
                        new ContextMenuButton(Environment, "Create File", CreateFile),
                    }), true, false),
                    new EmptySpace(Environment,  1, Spacing),
                }),
            });
        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; }

        public string Path { get; }
        public FileTypeManager FileTypeManager { get; set; }
        public PopupEnvironment PopupEnvironment { get; set; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }

        private IUiElement GenerateButton(CreatableFileType creatableFileType, bool selected)
        {
            int size = (int)Environment.FontHeight;
            IUiElement sprite = new SpriteView(Environment, FileTypeManager.GetIconPath("file" + creatableFileType.Extension));
            int index = _buttons.Count;
            string name = creatableFileType.Name + (creatableFileType.Extension == "" ? "" : (" (" + creatableFileType.Extension + ")"));
            ContextMenuButton button = new(Environment, name, () =>
            {
                foreach (ContextMenuButton b in _buttons)
                {
                    b.Disabled = false;
                }
                _selected = creatableFileType;
                _textInputField.Value = creatableFileType.Name;
                _textInputField.Extention = creatableFileType.Extension;
                _buttons[index].Disabled = true;
            })
            {
                Disabled = selected
            };
            _buttons.Add(button);

            return new HListView<IUiElement>(Environment, new List<IUiElement>()
            {
                new MinSize(Environment, new ScaleView(Environment, sprite), size, size),
                button
            });
        }

        private void CreateFile()
        {
            try
            {
                _selected.Creator.CreateFile(System.IO.Path.Join(Path, _textInputField.Value + _textInputField.Extention));
                PopupEnvironment.Close();
            }
            catch (Exception e)
            {
                PopupEnvironment.OpenCentered(new MessagePopup(Environment, e.Message, PopupEnvironment));
            }
        }
    }

}
