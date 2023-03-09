using MonoGameDrawingApp.VectorSprites.Export;
using System;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Applyable.Simple
{
    public class MoveModifier : SimpleModifier
    {
        private Vector2 _offset;

        public MoveModifier()
        {
            _offset = Vector2.Zero;
        }

        public override Action Changed { get; set; }
        public override Action Changing { get; set; }

        public Vector2 Offset
        {
            get => _offset;
            set
            {
                if (_offset != value)
                {
                    Changing?.Invoke();
                    _offset = value;
                    Changed?.Invoke();
                }
            }
        }

        protected override Polygon _modifyPolygon(Polygon polygon)
        {
            Vector2[] points = polygon.Vertices.ToArray();
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = points[i] + Offset;
            }
            return new Polygon(points, polygon.Color);
        }
    }
}
