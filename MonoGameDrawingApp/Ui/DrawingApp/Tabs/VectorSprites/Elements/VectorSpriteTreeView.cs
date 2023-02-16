using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Scroll;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;
using MonoGameDrawingApp.Ui.Base.Tree;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteTreeView : IUiElement
    {
        private readonly IUiElement _root;

        public VectorSpriteTreeView(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;

            _root = new VSplitStandard(
                environment: Environment,
                first: new CenterView(
                    environment: Environment,
                    child: new ColorModifier(
                        environment: Environment,
                        child: new TextView(Environment, "Tree"),
                        color: Environment.Theme.DefaultTextColor
                    ),
                    centerHorizontal: true,
                    centerVertical: false
                ),
                second: new StackView(
                    environment: Environment,
                    children: new List<IUiElement>()
                    {
                        new ColorRect(Environment, Environment.Theme.MenuBackgorundColor),
                        new ScrollWindow(
                            environment: Environment,
                            child: new TreeView(Environment, 20, 1, vectorSpriteTabView.Tree, false)
                        ),
                    }
                ),
                splitPosition: -1
            );
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
    }
}
