using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Lists
{
    public class HScrollableListView : ScrollableListView
    {
        public HScrollableListView(UiEnvironment environment, IEnumerable<IUiElement> items, bool updateOutOfView, int spacing) : base(environment, items, updateOutOfView, spacing)
        {
        }

        protected override IEnumerable<(Rectangle, IUiElement)> PositionItems()
        {
            List<(Rectangle, IUiElement)> result = new();
            Point currentPosition = Point.Zero;

            foreach (IUiElement item in Items)
            {
                Point size = new(item.RequiredWidth, Math.Max(Height, item.RequiredHeight));
                result.Add((new Rectangle(currentPosition, size), item));

                currentPosition += new Point(item.RequiredWidth + Spacing, 0);
            }

            return result;
        }
    }
}
