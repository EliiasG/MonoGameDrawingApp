using MonoGameDrawingApp.VectorSprites.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Export
{
    public class SerializableGeometryList
    {
        public SerializableGeometryList(VectorSprite sprite)
        {
            List<SerializableVectorSpriteGeometry> geometryList = new();

            _addGeometry(sprite.Root, geometryList, Vector2.Zero);

            Geometries = geometryList.ToArray();
        }

        public SerializableGeometryList()
        {
        }
        public SerializableVectorSpriteGeometry[] Geometries { get; set; }

        private void _addGeometry(VectorSpriteItem item, List<SerializableVectorSpriteGeometry> geometryList, Vector2 position)
        {
            position += item.Position;
            foreach (VectorSpriteItem child in item.Children)
            {
                _addGeometry(child, geometryList, position);
            }

            SerializableVectorSpriteGeometry geometry = new()
            {
                Points = item.Geometry.Points.Select((Vector2 point) => new SerializablleVector2(point + position)).ToArray(),
                Color = new SerializableColor(item.Geometry.Color),
            };
        }
    }
}
