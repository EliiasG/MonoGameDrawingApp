using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenu.Items;
using MonoGameDrawingApp.Ui.List;
using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Scroll;
using MonoGameDrawingApp.Ui.Split.Vertical;
using MonoGameDrawingApp.Ui.Tabs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class DrawingAppStart : IUiElement
    {

        public readonly TabEnvironment TabEnvironment;
        public readonly PopupEnvironment PopupEnvironment;

        private static readonly string ProjectsPath = Path.Join(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "VectorDrawingApp", "projects.txt");

        private readonly UiEnvironment _environment;
        private readonly VScrollableListView _scrollableListView;
        private readonly IUiElement _root;

        public DrawingAppStart(UiEnvironment environment, PopupEnvironment popupEnvironment, TabEnvironment tabEnvironment)
        {
            _environment = environment;
            TabEnvironment = tabEnvironment;
            PopupEnvironment = popupEnvironment;
            _scrollableListView = new VScrollableListView(environment, new IUiElement[0], false, 2);
            ReloadProjects();
            Debug.WriteLine(ProjectsPath);
            _root = new VSplitStandard(
                environment: environment,
                first: new VListView<IUiElement>(Environment, new List<IUiElement>()
                {
                    new MinSize(Environment, new ColorRect(Environment, Color.Transparent), 1, 5),
                    new ContextMenuButton(Environment, "Import/Create Project", () => throw new NotImplementedException()),
                    new MinSize(Environment, new ColorRect(Environment, Color.Transparent), 1, 5),
                }),
                second: new StackView(environment, new List<IUiElement>() {
                    new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                    new ScrollWindow(Environment, _scrollableListView),
                }),
                splitPosition: 1
            );
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

        public void ReloadProjects()
        {
            List<IUiElement> items = new List<IUiElement>();
            if (!File.Exists(ProjectsPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ProjectsPath));
                File.Create(ProjectsPath).Close();
            }
            foreach (string line in File.ReadLines(ProjectsPath))
            {
                items.Add(_generateButton(line));
            }

            _scrollableListView.Items = items;
        }

        private IUiElement _generateButton(string path)
        {
            //TODO
            return new ContextMenuButton(Environment, path, () => Debug.WriteLine("TODO: open, " + path));
        }
    }
}
