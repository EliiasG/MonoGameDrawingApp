using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener;
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

        private bool _pressedSave;


        private KeyboardState _oldKeyboard;

        public VectorSpriteTabView(UiEnvironment environment, VectorSprite sprite, string path, PopupEnvironment popupEnvironment)
        {
            Environment = environment;
            Sprite = sprite;
            Sprite.AddAttachment(new ChangeListenerVectorSpriteAttachment(() => _pressedSave = false));
            _jsonSaver = new VectorSpriteJsonSaver(Sprite);
            Path = path;
            _pressedSave = true;
            PopupEnvironment = popupEnvironment;

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

        public PopupEnvironment PopupEnvironment { get; init; }

        public VectorSpriteTree Tree { get; init; }

        public VectorSprite Sprite { get; private set; }

        public bool IsSaved => _pressedSave && !_jsonSaver.CurrentlySaving;

        public bool IsSaving => _jsonSaver.CurrentlySaving;

        public string Path { get; init; }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; set; }

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
