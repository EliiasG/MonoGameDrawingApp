﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Lists
{
    public class VListView<T> : ListView<T> where T : IUiElement
    {
        private readonly RenderHelper _renderHelper;

        public VListView(List<T> items) : base(items)
        {
            _renderHelper = new RenderHelper();
        }

        public override int RequiredWidth
        {
            get
            {
                int result = 0;
                foreach (IUiElement item in Items)
                {
                    result = Math.Max(result, item.RequiredWidth);
                }
                return result;
            }
        }

        public override int RequiredHeight
        {
            get
            {
                int result = 0;
                foreach (IUiElement item in Items)
                {
                    result += item.RequiredHeight;
                }
                return result;
            }
        }

        public override Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);
            List<Texture2D> renders = new List<Texture2D>();

            Vector2 renderPosition = position;
            foreach (IUiElement item in Items)
            {
                renders.Add(item.Render(graphics, renderPosition, width, item.RequiredHeight));
                renderPosition += new Vector2(0, item.RequiredHeight);
            }

            renderPosition = Vector2.Zero;
            foreach (Texture2D render in renders)
            {
                graphics.SpriteBatch.Draw(
                    texture: render,
                    position: renderPosition,
                    color: Color.White
                );
                renderPosition += new Vector2(0, render.Height);
            }

            return _renderHelper.Finish();
        }
    }
}