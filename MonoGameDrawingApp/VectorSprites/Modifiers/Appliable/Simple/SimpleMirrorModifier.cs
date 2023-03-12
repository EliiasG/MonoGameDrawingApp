using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class SimpleMirrorModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<int> _indexOffset;

        public SimpleMirrorModifier()
        {
            _indexOffset = new GeometryModifierParameter<int>(0, "IndexOffset");
            Parameters = new IGeometryModifierParameter[]
            {
                _indexOffset
            };
        }

        public override string Name => "SimpleMirror";

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon _modifyPolygon(Polygon polygon)
        {
            Vector2 line1 = Util.GetItemCircled(polygon.Vertices, _indexOffset.Value);
            Vector2 line2 = Util.GetItemCircled(polygon.Vertices, _indexOffset.Value + 1);

            Vector2[] newVertices = new Vector2[polygon.Vertices.Length * 2 - 2];
            int cur = 0;
            for (int i = 0; i < polygon.Vertices.Length - 1; i++)
            {
                newVertices[cur++] = Util.GetItemCircled(polygon.Vertices, i + _indexOffset.Value + 1);
            }
            for (int i = 0; i < polygon.Vertices.Length - 1; i++)
            {
                Vector2 newVec = Util.GetItemCircled(polygon.Vertices, _indexOffset.Value - i);
                newVec = ExtraMath.Mirrored(line1, line2, newVec);
                newVertices[cur++] = newVec;
            }

            return new Polygon(newVertices, polygon.Color);
        }
    }
}
