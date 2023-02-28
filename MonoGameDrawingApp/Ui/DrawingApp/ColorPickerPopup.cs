using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.VectorSprites;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class ColorPickerPopup : IUiElement
    {
        private const int ScrollBarWidth = 7;
        private const int Spacing = 5;
        private const int InnerWidth = 400;
        private const int InnerHeight = 600;
        private const int ColorButtonSpacing = 4;
        private const int ColorButtonAmount = 16;
        private const int ColorButtonSize = (InnerWidth - ColorButtonSpacing - ScrollBarWidth) / ColorButtonAmount - ColorButtonSpacing;
        private const int TextHeight = 30;

        private readonly PopupEnvironment _internalPopupEnvironment;
        private readonly IUiElement _root;
        private readonly ContextMenuButton _paletteButton;

        private readonly List<IUiElement> _colorList;

        public ColorPickerPopup(UiEnvironment environment, PopupEnvironment popupEnvironment, System.Drawing.Color previousColor, Action<System.Drawing.Color> confirmed)
        {
            Environment = environment;
            PeviousColor = previousColor;
            Confirmed = confirmed;
            _colorList = new List<IUiElement>();

            _paletteButton = new ContextMenuButton(Environment, SaveState.SelectedColorPalette.Name, _selectPalette);

            _internalPopupEnvironment = _generateInternalPopupEnvironment();

            _root = _internalPopupEnvironment;
        }

        public System.Drawing.Color PeviousColor { get; init; }

        public Action<System.Drawing.Color> Confirmed { get; init; }

        public System.Drawing.Color Selected { get; set; } //TODO set preview

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

        private void _selectPalette()
        {
            List<IUiElement> items = new();

            foreach (ColorPalette colorPalette in SaveState.ColorPalettes)
            {
                items.Add(new ContextMenuButton(Environment, colorPalette.Name, () =>
                {
                    _internalPopupEnvironment.Close();
                    _setPalette(colorPalette);
                }));
            }

            items.Add(new ContextMenuSeperator(Environment));

            items.Add(new ContextMenuButton(Environment, "Manage Palettes", () =>
            {
                _internalPopupEnvironment.Close();
                IOHelper.OpenInExplorer(SaveState.PalettesDirectory);
            }));

            _internalPopupEnvironment.Open(new Point(Spacing), new ContextMenu(Environment, items, _internalPopupEnvironment));
        }

        private void _setPalette(ColorPalette colorPalette)
        {
            SaveState.SelectedColorPalette = colorPalette;
            _paletteButton.Title = colorPalette.Name;

            _colorList.Clear();

            _colorList.Add(new EmptySpace(Environment, 1, ColorButtonSpacing));

            List<IUiElement> row = null;

            int remaning = 0;

            foreach (System.Drawing.Color color in colorPalette.Colors)
            {
                if (remaning <= 0)
                {
                    if (row != null)
                    {
                        _colorList.Add(new HListView<IUiElement>(Environment, row));
                    }

                    row = new List<IUiElement>()
                    {
                        Capacity = ColorButtonAmount
                    };

                    remaning = ColorButtonAmount;
                }

                row.Add(new EmptySpace(Environment, ColorButtonSpacing, 1));

                row.Add(new SmartButton(
                    environment: Environment,
                    leftClicked: () => Selected = color,
                    child: new MinSize(
                        environment: Environment,
                        child: new ColorRect(Environment, Util.ToXnaColor(color)),
                        width: ColorButtonSize,
                        height: ColorButtonSize
                    )
                ));

                --remaning;
            }
        }

        private PopupEnvironment _generateInternalPopupEnvironment()
        {
            ScrollWindow scrollWindow = new ScrollWindow(
                environment: Environment,
                child: new VScrollableListView(
                    environment: Environment,
                    items: _colorList,
                    updateOutOfView: false,
                    spacing: ColorButtonSpacing
                )
            );

            scrollWindow.VScrollBar.Size = ScrollBarWidth;

            // beatiful code, huh?
            return new PopupEnvironment(
                environment: Environment,
                child: new StackView(
                    environment: Environment,
                    children: new List<IUiElement>
                    {
                        new ColorRect(Environment, Environment.Theme.SecondaryMenuBackgroundColor),
                        new VListView<IUiElement>(
                            environment: Environment,
                            items: new List<IUiElement>()
                            {
                                new EmptySpace(Environment, 1, Spacing),
                                _generateIndented(
                                    child: new MinSize(
                                        environment : Environment,
                                        child: new StackView(
                                            environment: Environment,
                                            children: new List<IUiElement>()
                                            {
                                                new ColorRect(Environment, Environment.Theme.ButtonColor),
                                                new CenterView(
                                                    environment: Environment,
                                                    child: _paletteButton,
                                                    centerHorizontal: false,
                                                    centerVertical: true
                                                ),
                                            }
                                        ),
                                        width: InnerWidth,
                                        height: TextHeight
                                    )
                                ),
                                new EmptySpace(Environment, 1, Spacing),
                                _generateIndented(
                                    child: new MinSize(
                                        environment: Environment,
                                        child: new StackView(
                                            environment: Environment,
                                            children: new List<IUiElement>()
                                            {
                                                new ColorRect(Environment, Environment.Theme.ButtonColor),
                                                scrollWindow,
                                            }
                                        ),
                                        width: InnerWidth,
                                        height: InnerHeight
                                    )
                                ),
                                new EmptySpace(Environment, 1, Spacing),
                            }
                        ),
                    }
                )
            );
        }

        private IUiElement _generateIndented(IUiElement child)
        {
            return new HListView<IUiElement>(
                environment: Environment,
                items: new List<IUiElement>()
                {
                    new EmptySpace(Environment, Spacing, 1),
                    child,
                    new EmptySpace(Environment, Spacing, 1),
                }
            );
        }
    }
}
