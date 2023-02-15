using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.Base.Tree;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments;
using MonoGameDrawingApp.VectorSprites.Serialization.Json;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites
{
    public class VectorSpriteTabView : IUiElement
    {
        private readonly IUiElement _root;
        private readonly HSplit _inner;
        private readonly HSplit _outer;
        private readonly VectorSpriteJsonSaver _jsonSaver;

        private bool _pressedSave;

        private static int s_innerSplit = 200;
        private static int s_outerSplit = 1000;

        private KeyboardState _oldKeyboard;

        public VectorSpriteTabView(UiEnvironment environment, string path, PopupEnvironment popupEnvironment)
        {
            Environment = environment;
            //TODO load from file instead
            Sprite = new VectorSprite();
            Sprite.Root = new VectorSpriteItem("Root", Sprite);
            Sprite.Root.AddChild(new VectorSpriteItem("Child", Sprite));
            Sprite.AddAttachment(new ChangeListenerVectorSpriteAttachment(() => _pressedSave = false));
            _jsonSaver = new VectorSpriteJsonSaver(Sprite);
            Path = path;
            _pressedSave = true;

            VectorSpriteTree tree = new(Sprite, popupEnvironment);

            IUiElement treeElement = new StackView(
                environment: Environment,
                children: new List<IUiElement>()
                {
                    new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                    new ScrollWindow(
                        environment: Environment,
                        child: new TreeView(Environment, 20, 1, tree, false)
                    ),
                }
            );

            IUiElement viewportElement = new CenterView(Environment, new TextView(Environment, "Viewport goes here"), true, true);

            IUiElement inspectorElement = new CenterView(Environment, new TextView(Environment, "Inspector goes here"), true, true);

            _inner = new HSplitDraggable(
                environment: Environment,
                first: treeElement,
                second: viewportElement,
                splitPosition: s_innerSplit,
                handleWidth: 10
            );

            _outer = new HSplitDraggable(
                environment: Environment,
                first: _inner,
                second: inspectorElement,
                splitPosition: s_outerSplit,
                handleWidth: 10
            );

            _root = _outer;
        }

        public VectorSprite Sprite { get; init; }

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

            _inner.SplitPosition = s_innerSplit;
            _outer.SplitPosition = s_outerSplit;

            if (keyboard.IsKeyDown(Keys.LeftControl) && keyboard.IsKeyDown(Keys.S) && !_oldKeyboard.IsKeyDown(Keys.S))
            {
                Save();
            }

            _root.Update(position, width, height);

            s_innerSplit = _inner.SplitPosition;
            s_outerSplit = _outer.SplitPosition;

            _oldKeyboard = Keyboard.GetState();
        }

        public void Save()
        {
            _jsonSaver.StartSaving(Path);
            _pressedSave = true;
        }
    }
}
