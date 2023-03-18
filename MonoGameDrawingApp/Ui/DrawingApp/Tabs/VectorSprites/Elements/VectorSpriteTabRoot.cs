using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Serialization.Json;
using System;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteTabRoot : IUiElement
    {

        private VectorSpriteTabView _vectorSpriteTabView;
        private readonly IUiElement _root;

        public VectorSpriteTabRoot(UiEnvironment environment, string path, PopupEnvironment popupEnvironment)
        {
            Environment = environment;

            TextView text = new(Environment, "Loading...");

            ChangeableView changeableView = new(
                environment: Environment,
                child: new ScrollWindow(
                    environment: Environment,
                    child: new PeekView(
                        environment: Environment,
                        child: new CenterView(
                            environment: Environment,
                            child: new ColorModifier(
                                environment: Environment,
                                child: text,
                                color: environment.Theme.DefaultTextColor
                            ),
                            centerHorizontal: true,
                            centerVertical: true
                        )
                    )
                )
            );

            _root = changeableView;

            Task.Run(() =>
            {
                try
                {
                    VectorSprite vectorSprite = new VectorSpriteJsonLoader().LoadVectorSprite(path);
                    _vectorSpriteTabView = new VectorSpriteTabView(Environment, vectorSprite, path, popupEnvironment);
                    changeableView.Child = _vectorSpriteTabView;
                }
                catch (Exception e)
                {
                    text.Text = "Failed to load '" + path + "':\n" + e.Message + "\n\n" + e.StackTrace;
                }
            });
        }

        public bool IsSaved => _vectorSpriteTabView?.IsSaved ?? true;

        public bool IsSaving => _vectorSpriteTabView?.IsSaving ?? false;

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
            _root.Update(position, width, height);
        }

        public void Save()
        {
            _vectorSpriteTabView?.Save();
        }
    }
}
