using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp
{
    public class TriangleBatch
    {
        private const int MaxVertexCount = 2048;
        private const int MaxIndexCount = MaxVertexCount * 3;

        private bool _isStarted;

        private int _vertexCount;
        private int _indexCount;

        private readonly VertexPositionColor[] _vertices;
        private readonly int[] _indices;

        public TriangleBatch(GraphicsDevice graphicsDevice, BasicEffect effect = null)
        {
            _vertices = new VertexPositionColor[MaxVertexCount];
            _indices = new int[MaxIndexCount];

            GraphicsDevice = graphicsDevice;
            Effect = effect ?? new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = false,
                FogEnabled = false,
                LightingEnabled = false,
                VertexColorEnabled = true,
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.Identity,
            };
        }

        public GraphicsDevice GraphicsDevice { get; init; }

        public BasicEffect Effect { get; init; }

        public void Begin()
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
            }
            _isStarted = true;
            _vertexCount = 0;
            _indexCount = 0;
        }

        public void DrawTriangles(VertexPositionColor[] vertexData, int[] indices)
        {
            if (!_isStarted)
            {
                throw new InvalidOperationException("Begin must be called before drawing");
            }

            if (vertexData.Length >= MaxVertexCount || indices.Length >= MaxIndexCount)
            {
                _forceDrawTriangles(vertexData, indices, vertexData.Length, indices.Length / 3);
                return;
            }

            if (_vertexCount + vertexData.Length > MaxVertexCount || _indexCount + indices.Length > MaxIndexCount)
            {
                _flush();
            }

            foreach (int index in indices)
            {
                _indices[_indexCount++] = index + _vertexCount;
            }

            foreach (VertexPositionColor vertex in vertexData)
            {
                _vertices[_vertexCount++] = vertex;
            }
        }

        public void End()
        {
            if (!_isStarted)
            {
                throw new InvalidOperationException("Begin must be called before calling End.");
            }
            _isStarted = false;
            _flush();
        }

        private void _flush()
        {
            _forceDrawTriangles(_vertices, _indices, _vertexCount, _indexCount / 3);
            _indexCount = 0;
            _vertexCount = 0;

            //no reason to clear array, whats after the _index- and _vertexCount does not matter
        }

        private void _forceDrawTriangles(VertexPositionColor[] vertexData, int[] indices, int vertexCount, int triangleCount)
        {
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives(
                    primitiveType: PrimitiveType.TriangleList,
                    vertexData: vertexData,
                    vertexOffset: 0,
                    numVertices: vertexCount,
                    indexData: indices,
                    indexOffset: 0,
                    primitiveCount: triangleCount
                );
            }
        }
    }
}
