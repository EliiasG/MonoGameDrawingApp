using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener;
using MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo;
using MonoGameDrawingApp.VectorSprites.Serialization.Json;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteTabView : IUiElement
    {
        private const int TreeSize = 300;
        private const int InspectorSize = 300;

        private readonly IUiElement _root;
        private readonly HSplit _inner;
        private readonly HSplit _outer;
        private readonly VectorSpriteJsonSaver _jsonSaver;
        private readonly UndoRedoVectorSpiteAttachment _undoRedoAttachment;

        private bool _pressedSave;
        private Action<VectorSpriteItem> _onSelected;
        private readonly Button _treeButton;
        private VectorSpriteTreeItem _selected;

        private MouseState _oldMouse;

        public VectorSpriteTabView(UiEnvironment environment, VectorSprite sprite, string path, PopupEnvironment popupEnvironment)
        {
            Environment = environment;
            Sprite = sprite;
            ChangeListener = new ChangeListenerVectorSpriteAttachment(() =>
            {
                _pressedSave = false;
                if (Selected?.Parent == null && sprite.Root != Selected)
                {
                    Tree.Selected = null;
                }
            });
            _undoRedoAttachment = new UndoRedoVectorSpiteAttachment();
            _jsonSaver = new VectorSpriteJsonSaver(Sprite);
            Path = path;
            _pressedSave = true;
            PopupEnvironment = popupEnvironment;

            Sprite.AddAttachment(ChangeListener);
            Sprite.AddAttachment(_undoRedoAttachment);

            Tree = new VectorSpriteTree(Sprite, popupEnvironment);

            _treeButton = new Button(Environment, new VectorSpriteTreeView(Environment, this));

            IUiElement viewportElement = new VectorSpriteViewportView(Environment, this);

            IUiElement inspectorElement = new VectorSpriteInspector(Environment, this);

            _inner = new HSplitStandard(
                environment: Environment,
                first: _treeButton,
                second: viewportElement,
                splitPosition: TreeSize
            );

            _outer = new HSplitStandard(
                environment: Environment,
                first: _inner,
                second: inspectorElement,
                splitPosition: InspectorSize
            );

            _root = _outer;
        }

        public VectorSpriteItem Selected => _selected?.Item;//(Tree.Selected as VectorSpriteTreeItem)?.Item;

        public ChangeListenerVectorSpriteAttachment ChangeListener { get; init; }

        public PopupEnvironment PopupEnvironment { get; init; }

        public VectorSpriteTree Tree { get; init; }

        public VectorSprite Sprite { get; init; }

        public bool IsSaved => _pressedSave && !_jsonSaver.CurrentlySaving;

        public bool IsSaving => _jsonSaver.CurrentlySaving;

        public string Path { get; init; }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            _inner.SplitPosition = TreeSize;
            _outer.SplitPosition = width - InspectorSize;

            if (keyboard.IsKeyDown(Keys.LeftControl) && Environment.JustPressed(Keys.S))
            {
                Save();
            }

            if (keyboard.IsKeyDown(Keys.LeftControl) && Environment.JustPressed(Keys.Z))
            {
                _undoRedoAttachment.Undo();
            }

            if (keyboard.IsKeyDown(Keys.LeftControl) && Environment.JustPressed(Keys.Y))
            {
                _undoRedoAttachment.Redo();
            }

            bool justLeft = _oldMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed;
            bool justRight = _oldMouse.RightButton == ButtonState.Released && mouse.RightButton == ButtonState.Pressed;

            if (_onSelected == null)
            {
                _selected = Tree.Selected as VectorSpriteTreeItem;
            }
            else if (Tree.Selected is VectorSpriteTreeItem item)
            {
                _onSelected(item.Item);
                _onSelected = null;
                Tree.Selected = _selected;
            }
            else if ((justLeft && !_treeButton.ContainsMouse) || justRight)
            {
                _onSelected(null);
                _onSelected = null;
                Tree.Selected = _selected;
            }

            _undoRedoAttachment.Tick();

            _root.Update(position, width, height);

            _oldMouse = Mouse.GetState();
        }

        public void SelectItem(Action<VectorSpriteItem> selected)
        {
            _onSelected = selected;
            Tree.Selected = null;
            _oldMouse = Mouse.GetState();
        }

        public void Save()
        {
            _jsonSaver.StartSaving(Path);
            _pressedSave = true;
        }
    }
}
