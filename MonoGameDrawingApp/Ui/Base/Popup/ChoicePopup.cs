using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Popup
{
    public class ChoicePopup : IUiElement
    {
        public readonly string Title;

        private const int Spacing = 5;
        private const int ButtonSpacing = 15;

        private readonly UiEnvironment _environment;
        private readonly PopupEnvironment _popupEnvironment;

        private readonly IUiElement _outer;

        public ChoicePopup(UiEnvironment environment, string title, PopupEnvironment popupEnvironment, ChoicePopupOption[] options)
        {
            Title = title;
            _environment = environment;
            _popupEnvironment = popupEnvironment;

            List<IUiElement> buttons = new();

            foreach (ChoicePopupOption option in options)
            {
                buttons.Add(new ContextMenuButton(environment, option.Name, () =>
                {
                    popupEnvironment.Close();
                    option.Selected();
                }));
            }

            HListView<IUiElement> buttonView = new(environment, buttons)
            {
                Spacing = ButtonSpacing
            };

            _outer = new StackView(environment, new List<IUiElement>()
            {
                new ColorRect(environment, environment.Theme.SecondaryMenuBackgroundColor),
                new VListView<IUiElement>(environment, new List<IUiElement>
                {
                    new EmptySpace(Environment,  1, Spacing),
                    new CenterView(environment, new HListView<IUiElement>(Environment, new List<IUiElement>
                    {
                        new EmptySpace(Environment,  Spacing, 1),
                        new ColorModifier(environment, new TextView(environment, title), environment.Theme.EditingTextColor),
                        new EmptySpace(Environment,  Spacing, 1),
                    }), true, false),
                    new EmptySpace(Environment,  1, Spacing * 2),
                    new CenterView(environment, buttonView, true, false),
                    new EmptySpace(Environment,  1, Spacing),
                }),
            });
        }

        public bool Changed => _outer.Changed;

        public int RequiredWidth => _outer.RequiredWidth;

        public int RequiredHeight => _outer.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _outer.Update(position, width, height);
        }
    }
}
