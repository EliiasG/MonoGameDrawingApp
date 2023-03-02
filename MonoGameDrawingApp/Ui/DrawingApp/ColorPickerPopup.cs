using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.TextInput;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using MonoGameDrawingApp.VectorSprites;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class ColorPickerPopup : IUiElement
    {
        private const string BackgroundPath = "colorBackground";
        private const int ScrollBarWidth = 7;
        private const int Spacing = 10;
        private const int InnerWidth = 400;
        private const int InnerHeight = 600;
        private const int ColorButtonSpacing = 4;
        private const int ColorButtonAmount = 8;
        private const int ColorButtonSize = (InnerWidth - ColorButtonSpacing - ScrollBarWidth) / ColorButtonAmount - ColorButtonSpacing;
        private const int ValueInputWidth = InnerWidth / 4 - Spacing * 3 / 4;
        private const int ColorViewWidth = InnerWidth / 3 - Spacing * 2 / 3;
        private const int ColorViewHeight = ColorViewWidth / 2;
        private const int ButtonHeight = (ColorViewHeight - Spacing) / 2;
        private const int TextHeight = 30;

        private readonly PopupEnvironment _internalPopupEnvironment;
        private readonly IUiElement _root;
        private readonly ContextMenuButton _paletteButton;
        private readonly TextInputField _rField;
        private readonly TextInputField _gField;
        private readonly TextInputField _bField;
        private readonly TextInputField _aField;
        private readonly ColorRect _newColorRect;

        private readonly List<IUiElement> _colorList;

        private Button _colorButton;

        private System.Drawing.Color _selected;

        public ColorPickerPopup(UiEnvironment environment, PopupEnvironment popupEnvironment, System.Drawing.Color previousColor, Action<System.Drawing.Color> confirmed)
        {
            Environment = environment;
            PreviousColor = previousColor;
            PopupEnvironment = popupEnvironment;
            Confirmed = confirmed;

            _colorList = new List<IUiElement>();

            _paletteButton = new ContextMenuButton(Environment, "Loading...", _selectPalette);
            _newColorRect = new ColorRect(Environment, Color.White);

            _rField = _generateValueField(Selected.R, (int v) => Selected = System.Drawing.Color.FromArgb(Selected.A, v, Selected.G, Selected.B));
            _gField = _generateValueField(Selected.G, (int v) => Selected = System.Drawing.Color.FromArgb(Selected.A, Selected.R, v, Selected.B));
            _bField = _generateValueField(Selected.B, (int v) => Selected = System.Drawing.Color.FromArgb(Selected.A, Selected.R, Selected.G, v));
            _aField = _generateValueField(Selected.A, (int v) => Selected = System.Drawing.Color.FromArgb(v, Selected.R, Selected.G, Selected.B));

            Selected = previousColor;

            _colorButton = _generateColorButton();

            _internalPopupEnvironment = _generateInternalPopupEnvironment();

            _root = _internalPopupEnvironment;

            _setPalette(SaveState.SelectedColorPalette);
        }

        public PopupEnvironment PopupEnvironment { get; init; }

        public System.Drawing.Color PreviousColor { get; init; }

        public Action<System.Drawing.Color> Confirmed { get; init; }

        public System.Drawing.Color Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    _rField.Value = "R: " + value.R;
                    _gField.Value = "G: " + value.G;
                    _bField.Value = "B: " + value.B;
                    _aField.Value = "A: " + value.A;
                    _newColorRect.Color = Util.ToXnaColor(value);
                }
            }
        }

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
                    leftClicked: () =>
                    {
                        if (_colorButton.ContainsMouse)
                        {
                            Selected = color;
                        }
                    },
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

        private Button _generateColorButton()
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

            return new Button(Environment, scrollWindow);
        }

        private PopupEnvironment _generateInternalPopupEnvironment()
        {
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
                                                new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                                                _colorButton,
                                            }
                                        ),
                                        width: InnerWidth,
                                        height: InnerHeight
                                    )
                                ),
                                new EmptySpace(Environment, 1, Spacing),
                                _generateIndented(
                                    child: new MinSize(
                                        environment: Environment,
                                        child: new HScrollableListView(
                                            environment: Environment,
                                            items: new List<IUiElement>()
                                            {
                                                new MinSize(
                                                    environment: Environment,
                                                    child: _rField,
                                                    width: ValueInputWidth,
                                                    height: 1
                                                ),
                                                new MinSize(
                                                    environment: Environment,
                                                    child: _gField,
                                                    width: ValueInputWidth,
                                                    height: 1
                                                ),
                                                new MinSize(
                                                    environment: Environment,
                                                    child: _bField,
                                                    width: ValueInputWidth,
                                                    height: 1
                                                ),
                                                new MinSize(
                                                    environment: Environment,
                                                    child: _aField,
                                                    width: ValueInputWidth,
                                                    height: 1
                                                ),
                                            },
                                            updateOutOfView: true,
                                            spacing: Spacing
                                        ),
                                        width: InnerWidth,
                                        height: TextHeight
                                    )
                                ),
                                new EmptySpace(Environment, 1, Spacing),
                                _generateIndented(
                                    child: new MinSize(
                                        environment: Environment,
                                        child: new HScrollableListView(
                                            environment: Environment,
                                            items: new List<IUiElement>()
                                            {
                                                new MinSize(
                                                    environment: Environment,
                                                    child: new StackView(
                                                        environment: Environment,
                                                        children: new List<IUiElement>()
                                                        {
                                                            new ScaleView(
                                                                environment: Environment,
                                                                child: new SpriteView(Environment, BackgroundPath),
                                                                disableBlur: true
                                                            ),
                                                            _newColorRect,
                                                        }
                                                    ),
                                                    width: ColorViewWidth,
                                                    height: ColorViewHeight
                                                ),
                                                new MinSize(
                                                    environment: Environment,
                                                    child: new StackView(
                                                        environment: Environment,
                                                        children: new List<IUiElement>()
                                                        {
                                                            new ScaleView(
                                                                environment: Environment,
                                                                child: new SpriteView(Environment, BackgroundPath),
                                                                disableBlur: true
                                                            ),
                                                            new ColorRect(Environment,Util.ToXnaColor(PreviousColor)),
                                                        }
                                                    ),
                                                    width: ColorViewWidth,
                                                    height: ColorViewHeight
                                                ),
                                                new VListView<IUiElement>(
                                                    environment: Environment,
                                                    items: new List<IUiElement>()
                                                    {
                                                        new MinSize(
                                                            environment: Environment,
                                                            child: new StackView(
                                                                environment: Environment,
                                                                children: new List<IUiElement>()
                                                                {
                                                                    new ColorRect(Environment, Environment.Theme.ButtonColor),
                                                                    new CenterView(
                                                                        environment: Environment,
                                                                        child: new ContextMenuButton(
                                                                            environment: Environment,
                                                                            title: "Confirm",
                                                                            onClick: () =>
                                                                            {
                                                                                PopupEnvironment.Close();
                                                                                Confirmed(Selected);
                                                                            }
                                                                        ),
                                                                        centerHorizontal: true,
                                                                        centerVertical: true
                                                                    )
                                                                }
                                                            ),
                                                            width: ColorViewWidth,
                                                            height: ButtonHeight
                                                        ),
                                                        new EmptySpace(Environment, 1, Spacing),
                                                        new MinSize(
                                                            environment: Environment,
                                                            child: new StackView(
                                                                environment: Environment,
                                                                children: new List<IUiElement>()
                                                                {
                                                                    new ColorRect(Environment, Environment.Theme.ButtonColor),
                                                                    new CenterView(
                                                                        environment: Environment,
                                                                        child: new ContextMenuButton(
                                                                            environment: Environment,
                                                                            title: "Cancel",
                                                                            onClick: () =>
                                                                            {
                                                                                PopupEnvironment.Close();
                                                                            }
                                                                        ),
                                                                        centerHorizontal: true,
                                                                        centerVertical: true
                                                                    )
                                                                }
                                                            ),
                                                            width: ColorViewWidth,
                                                            height: ButtonHeight
                                                        ),
                                                    }
                                                )
                                            },
                                            updateOutOfView: true,
                                            spacing: Spacing
                                        ),
                                        width: InnerWidth,
                                        height: ColorViewHeight
                                    )
                                ),
                                new EmptySpace(Environment, 1, Spacing),
                            }
                        ),
                    }
                )
            );
        }

        private TextInputField _generateValueField(int value, Action<int> changed)
        {
            TextInputField newField = new(
                environment: Environment,
                value: "",
                filters: new ITextInputFilter[] { new NumericTextInputFilter() }
            );

            string tmpVal = "";

            newField.Selected = () =>
            {
                tmpVal = newField.Value;
                newField.Value = "";
            };

            newField.Deselected = () =>
            {
                newField.Value = tmpVal;
            };

            newField.TextEntered = () =>
            {
                try
                {
                    int v = int.Parse(newField.Value);
                    v = Math.Clamp(v, 0, 255);
                    changed(v);
                }
                catch
                {
                    newField.Value = tmpVal;
                }
            };

            return newField;
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