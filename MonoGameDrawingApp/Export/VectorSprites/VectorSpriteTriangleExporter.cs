using MonoGameDrawingApp.VectorSprites.Export.Triangulation;
using System.Drawing;
using System.IO;
using System.Numerics;

namespace MonoGameDrawingApp.Export.VectorSprites
{
    public class VectorSpriteTriangleExporter : VectorSpriteExporter
    {
        public override string OutputExtention => "tris";

        protected override void ExportSprite(TriangulatedVectorSprite sprite, string exportFilePath)
        {
            using BinaryWriter writer = new(File.OpenWrite(exportFilePath));

            Color? old = null;

            for (int i = 0; i < sprite.Vertices.Length; i++)
            {
                Color color = sprite.Colors[i];

                bool changedColor = color != old;

                byte instuction = (byte)(changedColor ? 1 : 0);

                if (i > 0)
                {
                    writer.Write(instuction);
                }

                if (changedColor)
                {
                    writer.Write(color.A);
                    writer.Write(color.R);
                    writer.Write(color.G);
                    writer.Write(color.B);
                }

                WriteVector2(writer, sprite.Vertices[i]);

                old = color;
            }

            writer.Write((byte)2);

            foreach (int index in sprite.Indices)
            {
                writer.Write(index);
            }
        }

        private static void WriteVector2(BinaryWriter writer, Vector2 vector2)
        {
            writer.Write(vector2.X);
            writer.Write(vector2.Y);
        }
    }
}
