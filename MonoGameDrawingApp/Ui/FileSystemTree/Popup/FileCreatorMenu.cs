using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenu.Items;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes;
using MonoGameDrawingApp.Ui.List;
using MonoGameDrawingApp.Ui.TextInput;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.FileSystemTree.Popup
{

    public class FileCreatorMenu : IUiElement
    {
        public readonly string Path;

        public FileTypeManager _fileTypeManager;

        private readonly IUiElement _root;
        private readonly TextInputField _textInputField;
        private readonly UiEnvironment _environment;
        private readonly List<ContextMenuButton> _buttons;

        private CreatableFileType _selected;


        public FileCreatorMenu(UiEnvironment environment, FileTypeManager fileTypeManager)
        {
            _environment = environment;
            _fileTypeManager = fileTypeManager;
            _selected = new CreatableFileType(new EmptyFileCreator(), "Other", "");

            List<IUiElement> typeButtons = new List<IUiElement>();

            foreach (CreatableFileType creatableFileType in fileTypeManager.CreatableFileTypes)
            {
                typeButtons.Add(_generateButton(creatableFileType));
            }

            _textInputField = new TextInputField(Environment, _selected.Name, new ITextInputFilter[] { new FileSystemTextInputFilter() }, false, false, false, 100);

            _textInputField.IsSelected = true;
            _textInputField.Extention = _selected.Extention;

            typeButtons.Add(_generateButton(_selected));

            //TODO make layout:
            /*
             * ########
             * ##LIST##
             * ##LIST##
             * ##LIST##
             * ########
             * ##NAME##
             * #CNCL#OK
             */
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

        private IUiElement _generateButton(CreatableFileType creatableFileType)
        {
            int size = (int)Environment.FontHeight;
            IUiElement sprite = new SpriteView(Environment, _fileTypeManager.GetIconPath("file" + creatableFileType.Extention));
            int index = _buttons.Count;
            ContextMenuButton button = new ContextMenuButton(Environment, creatableFileType.Name, () =>
            {
                foreach (ContextMenuButton b in _buttons)
                {
                    b.Disabled = false;
                }
                _selected = creatableFileType;
                _buttons[index].Disabled = true;
            });

            _buttons.Add(button);

            return new HListView<IUiElement>(Environment, new List<IUiElement>()
            {
                new MinSize(Environment, new ScaleView(Environment, sprite), size, size),
                button
            });
        }
    }

}
