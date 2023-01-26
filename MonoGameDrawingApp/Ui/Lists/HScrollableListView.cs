using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.List
{
    public class HScrollableListView : ScrollableListView
    {
        public HScrollableListView(UiEnvironment environment, IEnumerable<IUiElement> items, bool updateOutOfView, int spacing) : base(environment, items, updateOutOfView, spacing)
        {
        }

        protected override IEnumerable<(Rectangle, IUiElement)> _positionItems()
        {
            List<(Rectangle, IUiElement)> result = new List<(Rectangle, IUiElement)>();
            Point currentPosition = Point.Zero;

            foreach (IUiElement item in Items)
            {
                Point size = new Point(item.RequiredWidth, Math.Max(Height, item.RequiredHeight));
                result.Add((new Rectangle(currentPosition, size), item));

                currentPosition += new Point(item.RequiredWidth + Spacing, 0);
            }

            return result;
        }
    }
}
