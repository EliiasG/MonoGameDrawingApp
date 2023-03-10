using MonoGameDrawingApp.VectorSprites.Export;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSpriteGeometry
    {
        private List<Vector2> _points;

        private Color _color;

        public VectorSpriteGeometry() : this(Array.Empty<Vector2>()) { }

        public VectorSpriteGeometry(IEnumerable<Vector2> points)
        {
            _points = new List<Vector2>(points);
            Color = Color.White;
        }

        public VectorSpriteItem VectorSpriteItem { get; set; }

        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    VectorSpriteItem?._dataChanging();
                    _color = value;
                    VectorSpriteItem?._dataChanged();
                }
            }
        }

        public IEnumerable<Vector2> Points
        {
            get => _points;
            set
            {
                VectorSpriteItem?._dataChanging();
                _points = new List<Vector2>(value);
                VectorSpriteItem?._dataChanged();
            }
        }

        public void AddPoint(Vector2 point)
        {
            VectorSpriteItem?._dataChanging();
            _points.Add(point);
            VectorSpriteItem?._dataChanged();
        }

        public void RemovePointAt(int index)
        {
            VectorSpriteItem?._dataChanging();
            _points.RemoveAt(index);
            VectorSpriteItem?._dataChanged();
        }

        public Vector2 PointAt(int pointIndex)
        {
            return _points[pointIndex];
        }

        public void InsertPointAt(int index, Vector2 point)
        {
            VectorSpriteItem?._dataChanging();
            _points.Insert(index, point);
            VectorSpriteItem?._dataChanged();
        }

        public void ReplacePointAt(int index, Vector2 point)
        {
            VectorSpriteItem?._dataChanging();
            _points[index] = point;
            VectorSpriteItem?._dataChanged();
        }

        public int Size => _points.Count;

        public Polygon ToPolygon()
        {
            return new Polygon(Points.ToArray(), Color);
        }
    }
}
