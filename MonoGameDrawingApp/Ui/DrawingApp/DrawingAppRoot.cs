using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tabs;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites;
using MonoGameDrawingApp.Ui.FileSystemTrees;
using MonoGameDrawingApp.Ui.FileSystemTrees.MiscFileTypes;
using MonoGameDrawingApp.Ui.FileSystemTrees.MiscFileTypes.Image;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class DrawingAppRoot : IUiElement
    {
        public readonly TabEnvironment TabEnvironment;
        public readonly PopupEnvironment PopupEnvironment;
        public readonly FileTypeManager FileTypeManager;

        private readonly UiEnvironment _environment;

        public DrawingAppRoot(UiEnvironment environment)
        {
            _environment = environment;
            SaveState.Init(environment.Content, environment.Graphics.Device);
            TabEnvironment = new TabEnvironment(Environment, new DrawingAppStart(Environment, this));
            PopupEnvironment = new PopupEnvironment(Environment, TabEnvironment);
            FileTypeManager = new FileTypeManager(
                tabEnvironment: TabEnvironment,
                fileTypes: new IOpenableFileType[]
                {
                    new PixelImageOpenableFileType(),
                    new VectorSpriteOpenableFileType(PopupEnvironment)
                },
                extensionIconCollections: new IExtensionIconCollection[]
                {
                    new DefaultExtensionIconCollection(),
                    new DrawingAppIconCollection(),
                },
                creatableTypes: new CreatableFileType[]
                {
                    new CreatableFileType(new EmptyFileCreator(), "Text File", ".txt"),
                    new CreatableFileType(new VectorSpriteFileCreator(), "Vector Sptite", ".vecspr")
                },
                openUnknownFiles: true
            );
        }

        public bool Changed => PopupEnvironment.Changed;

        public int RequiredWidth => PopupEnvironment.RequiredWidth;

        public int RequiredHeight => PopupEnvironment.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return PopupEnvironment.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            PopupEnvironment.Update(position, width, height);
        }
    }
}
