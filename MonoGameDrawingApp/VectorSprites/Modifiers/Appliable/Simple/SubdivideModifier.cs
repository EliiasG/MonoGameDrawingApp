using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class SubdivideModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<int> _resolution;

        public SubdivideModifier()
        {
            _resolution = new GeometryModifierParameter<int>(1, "Resolution", (int v) => Math.Clamp(v, 1, 8));
            Parameters = new IGeometryModifierParameter[]
            {
                _resolution
            };
        }


        public override string Name => "Subdivide";

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon _modifyPolygon(Polygon polygon)
        {
            Vector2[] vertices = polygon.Vertices;

            for (int i = 0; i < _resolution.Value; i++)
            {
                Vector2[] newVertices = new Vector2[vertices.Length * 2];

                for (int j = 0; j < vertices.Length; j++)
                {
                    Vector2 prev = Util.GetItemCircled(vertices, j - 1);
                    Vector2 cur = vertices[j];
                    Vector2 next = Util.GetItemCircled(vertices, j + 1);

                    newVertices[j * 2] = Vector2.Lerp(cur, prev, .25f);
                    newVertices[j * 2 + 1] = Vector2.Lerp(cur, next, .25f);
                }

                vertices = newVertices;
            }

            return new Polygon(vertices, polygon.Color);
        }
    }
}
