using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener;
using MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo;
using MonoGameDrawingApp.VectorSprites.Serialization.Json;

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

        private KeyboardState _oldKeyboard;

        public VectorSpriteTabView(UiEnvironment environment, VectorSprite sprite, string path, PopupEnvironment popupEnvironment)
        {
            Environment = environment;
            Sprite = sprite;
            ChangeListener = new ChangeListenerVectorSpriteAttachment(() => _pressedSave = false);
            _undoRedoAttachment = new UndoRedoVectorSpiteAttachment();
            _jsonSaver = new VectorSpriteJsonSaver(Sprite);
            Path = path;
            _pressedSave = true;
            PopupEnvironment = popupEnvironment;

            Sprite.AddAttachment(ChangeListener);
            Sprite.AddAttachment(_undoRedoAttachment);

            Tree = new VectorSpriteTree(Sprite, popupEnvironment);

            IUiElement treeElement = new VectorSpriteTreeView(Environment, this);

            IUiElement viewportElement = new VectorSpriteViewportView(Environment, this);//new CenterView(Environment, new TextView(Environment, "Viewport goes here"), true, true);

            IUiElement inspectorElement = new VectorSpriteInspector(Environment, this);

            _inner = new HSplitStandard(
                environment: Environment,
                first: treeElement,
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

        public VectorSpriteItem Selected => (Tree.Selected as VectorSpriteTreeItem)?.Item;

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

            _inner.SplitPosition = TreeSize;
            _outer.SplitPosition = width - InspectorSize;

            if (keyboard.IsKeyDown(Keys.LeftControl) && keyboard.IsKeyDown(Keys.S) && !_oldKeyboard.IsKeyDown(Keys.S))
            {
                Save();
            }

            if (keyboard.IsKeyDown(Keys.LeftControl) && keyboard.IsKeyDown(Keys.Z) && !_oldKeyboard.IsKeyDown(Keys.Z))
            {
                _undoRedoAttachment.Undo();
            }

            if (keyboard.IsKeyDown(Keys.LeftControl) && keyboard.IsKeyDown(Keys.Y) && !_oldKeyboard.IsKeyDown(Keys.Y))
            {
                _undoRedoAttachment.Redo();
            }

            _undoRedoAttachment.Tick();

            _root.Update(position, width, height);

            _oldKeyboard = Keyboard.GetState();
        }

        public void Save()
        {
            _jsonSaver.StartSaving(Path);
            _pressedSave = true;
        }
    }
}
