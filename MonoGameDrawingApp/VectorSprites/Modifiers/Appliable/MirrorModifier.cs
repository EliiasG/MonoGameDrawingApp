using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable
{
    public class MirrorModifier : IAppliableGeometryModifier
    {
        public string Name => "Mirror";

        private readonly GeometryModifierParameter<VectorSpriteItemReference> _mirrorLine;

        public MirrorModifier()
        {
            _mirrorLine = new GeometryModifierParameter<VectorSpriteItemReference>(new VectorSpriteItemReference(), "Line");
            Parameters = new IGeometryModifierParameter[]
            {
                _mirrorLine
            };
        }

        public IEnumerable<IGeometryModifierParameter> Parameters { get; }

        public void Apply(VectorSpriteItem item)
        {
            VectorSpriteItem newItem = new VectorSpriteItem("Mirrored" + item.Name, item.Sprite);
            newItem.Geometry.Points = _mirror(item.Geometry.Points, item.AbsolutePosition);
            newItem.Geometry.Color = item.Geometry.Color;

            if (item.Parent is VectorSpriteItem parent)
            {
                newItem.Position = item.Position;
                parent.AddChild(newItem);
            }
            else
            {
                item.AddChild(newItem);
            }
        }

        public void Modify(ModifiedGeometry geometry)
        {
            IEnumerable<Vector2> points = _mirrorLine.Value.Item?.Geometry.Points;
            if ((points?.Count() ?? 0) < 1)
            {
                return;
            }
            int c = geometry.ModifiedPolygons.Count;
            for (int i = 0; i < c; i++)
            {
                Polygon polygon = geometry.ModifiedPolygons[i * 2];
                geometry.ModifiedPolygons.Insert(i * 2 + 1, new Polygon(_mirror(polygon.Vertices, geometry.Position), polygon.Color));
            }
        }

        private Vector2[] _mirror(IEnumerable<Vector2> points, Vector2 position)
        {
            Vector2 first = _mirrorLine.Value.Item.AbsolutePosition - position;
            Vector2 second = first + _mirrorLine.Value.Item.Geometry.PointAt(0);
            Vector2[] vertices = new Vector2[points.Count()];
            int i = 0;
            foreach (Vector2 point in points)
            {
                vertices[i] = ExtraMath.Mirrored(first, second, point);
                ++i;
            }
            return vertices;
        }
    }
}
