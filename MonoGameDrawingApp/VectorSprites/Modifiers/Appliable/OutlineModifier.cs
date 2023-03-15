using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;
using System.Drawing;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable
{
    public class OutlineModifier : IAppliableGeometryModifier
    {
        private readonly GeometryModifierParameter<float> _size;
        private readonly GeometryModifierParameter<Color> _color;


        public string Name => "Outline";

        public OutlineModifier()
        {
            _size = new GeometryModifierParameter<float>(0, "Size");
            _color = new GeometryModifierParameter<Color>(Color.Black, "Color");
            Parameters = new IGeometryModifierParameter[]
            {
                _size,
                _color,
            };
        }

        public IEnumerable<IGeometryModifierParameter> Parameters { get; }

        public void Apply(VectorSpriteItem item)
        {
            VectorSpriteItem newItem = new("Foreground", item.Sprite)
            {
                Modifiers = item.Modifiers
            };

            newItem.RemoveModifier(this);

            newItem.Geometry.Points = item.Geometry.Points;

            item.Modifiers = new IGeometryModifier[]
            {
                new ExpandModifier(_size.Value)
            };

            item.Geometry.Color = _color.Value;
            item.InsertChild(0, newItem);
        }

        public void Modify(ModifiedGeometry geometry)
        {
            List<Polygon> polygons = geometry.ModifiedPolygons;
            Polygon first = polygons[0];
            if (first.Vertices.Length >= 3)
            {
                polygons.Add(new Polygon(ExtraMath.Expand(first.Vertices, _size.Value), _color.Value));
            }
        }
    }
}
