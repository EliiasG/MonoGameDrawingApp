using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Export;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tabs;
using System;
using System.IO;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.Project
{
    public class ProjectTab : Tab
    {
        public readonly DrawingAppRoot Root;

        private readonly IUiElement _child;
        private readonly GlobalShortcut _exportShortcut;


        public ProjectTab(DrawingAppRoot root, string path)
        {
            Root = root;
            _child = new ProjectTabView(root.Environment, root, path);

            ProjectExporter exporter = new(Path.Join(path, "CreationTimes.txt"), Path.Join(path, "Profiles.json"), Path.Join(path, "Source"));

            _exportShortcut = new GlobalShortcut(new Keys[] { Keys.LeftControl, Keys.E, }, () =>
            {
                Root.PopupEnvironment.OpenCentered(new StaticPopup(root.Environment, "Exporting..."));
                try
                {
                    exporter.Export(path + "Export");
                    Root.PopupEnvironment.Close();
                }
                catch (Exception e)
                {
                    root.PopupEnvironment.OpenCentered(new MessagePopup(root.Environment, e.Message, root.PopupEnvironment));
                }
            });

            root.Environment.AddShortcut(_exportShortcut);
        }

        public override IUiElement Child => _child;

        public override bool HasCloseButton => true;

        public override string Title => "Project";

        protected override void _close()
        {
            Root.PopupEnvironment.OpenCentered(new ChoicePopup(Root.Environment, "Close project? (will not save files!)", Root.PopupEnvironment, new ChoicePopupOption[]
            {
                new ChoicePopupOption("Confirm", () =>
                {
                    Root.Environment.RemoveShortcut(_exportShortcut);
                    while(true)
                    {
                        Tab selected = Root.TabEnvironment.TabBar.SelectedTab;
                        if(selected == null)
                        {
                            break;
                        }
                        selected.ForceClose();
                    }
                }),
                new ChoicePopupOption("Cancel", () => Root.PopupEnvironment.Close()),
            }));
        }
    }
}
