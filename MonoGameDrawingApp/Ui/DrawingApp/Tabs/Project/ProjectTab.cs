using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tabs;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.Project
{
    public class ProjectTab : Tab
    {
        public readonly DrawingAppRoot Root;

        private readonly IUiElement _child;

        public ProjectTab(DrawingAppRoot root, string path)
        {
            Root = root;
            _child = new ProjectTabView(root.Environment, root, path);
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
