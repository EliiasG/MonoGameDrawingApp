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
            Color = Color.White;
        }

        public VectorSpriteItem VectorSpriteItem { get; set; }

        public Color Color { get; set; }

        public IEnumerable<Vector2> Points
        {
            get => _points;
            set
            {
                _points = new List<Vector2>(value);
                VectorSpriteItem?._dataChanged();
            }
        }

        public void AddPoint(Vector2 point)
        {
            _points.Add(point);
            VectorSpriteItem?._dataChanged();
        }

        public void RemovePointAt(int index)
        {
            _points.RemoveAt(index);
            VectorSpriteItem?._dataChanged();
        }

        public Vector2 PointAt(int pointIndex)
        {
            return _points[pointIndex];
        }

        public void InsertPointAt(int index, Vector2 point)
        {
            _points.Insert(index, point);
            VectorSpriteItem?._dataChanged();
        }

        public void ReplacePointAt(int index, Vector2 point)
        {
            _points[index] = point;
            VectorSpriteItem?._dataChanged();
        }

        public int Size => _points.Count;
    }
}
