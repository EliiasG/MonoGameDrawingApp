using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Lists
{
    public class VScrollableListView : ScrollableListView
    {
        public VScrollableListView(UiEnvironment environment, IEnumerable<IUiElement> items, bool updateOutOfView, int spacing) : base(environment, items, updateOutOfView, spacing)
        {
        }

        protected override IEnumerable<(Rectangle, IUiElement)> _positionItems()
        {
            List<(Rectangle, IUiElement)> result = new List<(Rectangle, IUiElement)>();
            Point currentPosition = Point.Zero;

            foreach (IUiElement item in Items)
            {
                Point size = new Point(Math.Max(Width, item.RequiredWidth), item.RequiredHeight);
                result.Add((new Rectangle(currentPosition, size), item));

                currentPosition += new Point(0, item.RequiredHeight + Spacing);
            }

            return result;
        }
    }
}
