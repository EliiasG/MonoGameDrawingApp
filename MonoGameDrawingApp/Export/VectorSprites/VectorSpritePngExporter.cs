using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.VectorSprites.Export.Triangulation;
using MonoGameDrawingApp.VectorSprites.Rendering.MonoGame;
using System;
using System.IO;

namespace MonoGameDrawingApp.Export.VectorSprites
{
    public class VectorSpritePngExporter : VectorSpriteExporter
    {

        private const float Allowance = 0.001f;

        public VectorSpritePngExporter(int unitPixelSize, bool roundUnits, Graphics graphics)
        {
            UnitPixelSize = unitPixelSize;
            RoundUnits = roundUnits;
            Graphics = graphics;
        }

        public override string OutputExtention => "png";

        public Graphics Graphics { get; }

        public int UnitPixelSize { get; }

        public bool RoundUnits { get; }


        protected override void _exportSprite(TriangulatedVectorSprite sprite, string exportFilePath)
        {
            float left = 0, right = 0, top = 0, bottom = 0;

            foreach (Vector2 vertex in sprite.Vertices)
            {
                top = MathF.Max(top, vertex.Y);
                bottom = MathF.Min(bottom, vertex.Y);
                right = MathF.Max(right, vertex.X);
                left = MathF.Min(left, vertex.X);
            }

            if (RoundUnits)
            {
                top = MathF.Ceiling(top - Allowance);
                bottom = MathF.Floor(bottom + Allowance);
                right = MathF.Ceiling(right - Allowance);
                left = MathF.Floor(left + Allowance);
            }

            float width = Math.Max(right - left, 1);
            float height = Math.Max(top - bottom, 1);

            int imageWidth = (int)MathF.Ceiling(width * UnitPixelSize);
            int imageHeight = (int)MathF.Ceiling(height * UnitPixelSize);

            using RenderTarget2D renderTarget = new(Graphics.Device, imageWidth, imageHeight);

            Graphics.Device.SetRenderTarget(renderTarget);
            Graphics.Device.Clear(Color.Transparent);
            Graphics.TriangleBatch.Begin();

            Camera camera = new(UnitPixelSize, new Vector2(width / 2, height / 2));
            camera.Apply(Graphics.TriangleBatch.Effect, imageWidth, imageHeight);

            MonoGameTriangulatedVectorSpriteRenderer.Render(Graphics.TriangleBatch, sprite, new Vector2(-left, -bottom));

            Graphics.TriangleBatch.End();

            using Stream stream = File.OpenWrite(exportFilePath);

            renderTarget.SaveAsPng(stream, imageWidth, imageHeight);

            Graphics.Device.SetRenderTarget(null);
        }
    }
}
