using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Tabs;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.ProjectImporter
{
    public class ProjectImporterTab : Tab
    {
        public ProjectImporterTab(DrawingAppStart start)
        {
            Child = new ProjectImporterTabView(start.Environment, start, this);
        }

        public override IUiElement Child { get; }

        public override bool HasCloseButton => true;

        public override string Title => "Import Project";

        protected override void _close()
        {
            ForceClose();
        }
    }
}
