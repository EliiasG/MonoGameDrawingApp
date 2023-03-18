using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.VectorSprites;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements.Inspector.Properties
{
    public class VectorSpriteItemInspectorProperty : IInspectorProperty<VectorSpriteItem>
    {
        private readonly TextView _textView;
        private readonly IUiElement _root;

        private VectorSpriteItem _item;

        private IUiElement _background;
        private IUiElement _backgroundSelected;
        private ChangeableView _backgroundView;

        public VectorSpriteItemInspectorProperty(UiEnvironment environment, string name, VectorSpriteItem value, VectorSpriteTabView vectorSpriteTabView, Action changed)
        {
            Environment = environment;


            _textView = new TextView(Environment, "...");
            ValueChanged = changed;

            _background = new ColorRect(Environment, Environment.Theme.ButtonColor);
            _backgroundSelected = new ColorRect(Environment, Environment.Theme.SelectedButtonColor);

            _backgroundView = new ChangeableView(Environment, _background);

            _root = new NamedInspectorProperty(
                environment: Environment,
                child: new MinSize(
                    environment: Environment,
                    child: new SmartButton(
                        environment: Environment,
                        child: new StackView(
                            environment: Environment,
                            children: new IUiElement[]
                            {
                                _backgroundView,
                                new PeekView(
                                    environment: environment,
                                    child: new CenterView(
                                        environment: Environment,
                                        child: new ColorModifier(Environment, _textView, Environment.Theme.DefaultTextColor),
                                        centerHorizontal: false,
                                        centerVertical: true
                                    )
                                ),
                            }
                        ),
                        leftClicked: () =>
                        {
                            _textView.Text = "Select item...";
                            _backgroundView.Child = _backgroundSelected;
                            vectorSpriteTabView.SelectItem((VectorSpriteItem item) =>
                            {
                                if (item != null)
                                {
                                    Value = item;
                                    ValueChanged?.Invoke();
                                }
                                else
                                {
                                    _setText();
                                }
                                _backgroundView.Child = _background;
                            });
                        }
                    ),
                    width: 200,
                    height: 30
                ),
                text: name
            );

            Value = value;
        }

        public VectorSpriteItem Value
        {
            get => _item;
            set
            {
                _item = value;
                _setText();
            }
        }

        public Action ValueChanged { get; set; }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; set; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }

        private void _setText()
        {
            _textView.Text = Value?.Name ?? "Click to select";
        }
    }
}
