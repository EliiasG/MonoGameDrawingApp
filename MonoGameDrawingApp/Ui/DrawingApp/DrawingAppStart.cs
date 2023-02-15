using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.Project;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.ProjectImporter;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class DrawingAppStart : IUiElement
    {

        public readonly DrawingAppRoot Root;

        private static readonly string ProjectsPath = Path.Join(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "VectorDrawingApp", "projects.txt");

        private readonly UiEnvironment _environment;
        private readonly VScrollableListView _scrollableListView;
        private readonly IUiElement _root;

        public DrawingAppStart(UiEnvironment environment, DrawingAppRoot root)
        {
            _environment = environment;
            Root = root;
            _scrollableListView = new VScrollableListView(environment, new IUiElement[0], false, 2);
            ReloadProjects();
            Debug.WriteLine(ProjectsPath);
            _root = new VSplitStandard(
                environment: Environment,
                first: new VListView<IUiElement>(Environment, new List<IUiElement>()
                {
                    new MinSize(Environment, new ColorRect(Environment, Color.Transparent), 1, 5),
                    new ContextMenuButton(Environment, "Import/Create Project", _import),
                    new MinSize(Environment, new ColorRect(Environment, Color.Transparent), 1, 5),
                }),
                second: new StackView(Environment, new List<IUiElement>() {
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
            List<IUiElement> items = new();
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

        public void Remove(string path)
        {
            List<string> lines = File.ReadLines(ProjectsPath).ToList();
            while (lines.Remove(path)) ;
            File.WriteAllLines(ProjectsPath, lines.ToArray());
            ReloadProjects();
        }

        public void SetFirst(string path)
        {
            List<string> lines = File.ReadLines(ProjectsPath).ToList();
            while (lines.Remove(path)) ;
            lines.Insert(0, path);
            File.WriteAllLines(ProjectsPath, lines.ToArray());
            ReloadProjects();
        }

        private IUiElement _generateButton(string path)
        {
            //TODO
            return new ContextMenuButton(Environment, path, () =>
            {
                Root.PopupEnvironment.OpenCentered(new ChoicePopup(Environment, path, Root.PopupEnvironment, new ChoicePopupOption[]
                {
                    new ChoicePopupOption("Open", () => _open(path)),
                    new ChoicePopupOption("Remove From List", () => Remove(path)),
                    new ChoicePopupOption("Cancel", () => Root.PopupEnvironment.Close()),
                }));
            });
        }

        private void _import()
        {
            Root.TabEnvironment.TabBar.OpenTab(new ProjectImporterTab(this), true);
        }

        private void _open(string path)
        {
            SetFirst(path);
            Root.TabEnvironment.TabBar.OpenTab(new ProjectTab(Root, path), true);
        }
    }
}
