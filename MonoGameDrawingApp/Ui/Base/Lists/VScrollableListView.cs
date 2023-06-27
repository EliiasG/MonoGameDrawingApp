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

        protected override IEnumerable<(Rectangle, IUiElement)> PositionItems()
        {
            List<(Rectangle, IUiElement)> result = new();
            Point currentPosition = Point.Zero;

            foreach (IUiElement item in Items)
            {
                Point size = new(Math.Max(Width, item.RequiredWidth), item.RequiredHeight);
                result.Add((new Rectangle(currentPosition, size), item));

                currentPosition += new Point(0, item.RequiredHeight + Spacing);
            }

            return result;
        }
    }
}
