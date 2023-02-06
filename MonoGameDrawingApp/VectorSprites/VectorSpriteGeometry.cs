using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSpriteGeometry
    {
        private List<Vector2> _points;

        public VectorSpriteGeometry() : this(Array.Empty<Vector2>()) { }

        public VectorSpriteGeometry(IEnumerable<Vector2> points)
        {
            _points = new List<Vector2>(points);
        }

        public Color Color { get; set; }

        public IEnumerable<Vector2> Points
        {
            get => _points;
            set => _points = new List<Vector2>(value);
        }

        public void AddPoint(Vector2 point)
        {
            _points.Add(point);
        }

        public void RemovePointAt(int index)
        {
            _points.RemoveAt(index);
        }

        public Vector2 PointAt(int pointIndex)
        {
            return _points[pointIndex];
        }

        public void InsertAt(int index, Vector2 point)
        {
            _points.Insert(index, point);
        }
    }
}
