using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSpriteItemReference
    {
        //TODO serialize

        private readonly string _initPath;
        private readonly VectorSprite _initSprite;
        private bool _createFomPath;
        private VectorSpriteItem _item;

        public VectorSpriteItemReference() : this(null)
        {
        }

        public VectorSpriteItemReference(VectorSpriteItem item)
        {
            _item = item;
            _createFomPath = false;
        }

        public VectorSpriteItemReference(string path, VectorSprite sprite)
        {
            _initPath = path;
            _initSprite = sprite;
            _createFomPath = true;
        }

        public VectorSpriteItem Item
        {
            get
            {
                if (_createFomPath)
                {
                    try
                    {
                        _loadFromPath();
                    }
                    catch
                    {
                        _item = null;
                    }
                }

                return _item;
            }
            set
            {
                _item = value;
            }
        }

        public string Path
        {
            get
            {
                List<int> indices = new List<int>();
                VectorSpriteItem item = _item;
                while (item != null)
                {
                    indices.Add(Array.IndexOf(item.Parent.Children.ToArray(), item));
                    item = item.Parent;
                }
                return string.Join("/", indices.Reverse<int>().ToArray());
            }
        }

        private void _loadFromPath()
        {
            if (_initPath == string.Empty)
            {
                _item = null;
                _createFomPath = false;
                return;
            }
            VectorSpriteItem item = _initSprite.Root;
            IEnumerable<int> indices = _initPath.Split('/').Select((string s) => int.Parse(s));
            foreach (int i in indices)
            {
                item = item.GetChild(i);
            }
            _item = item;
            _createFomPath = false;
        }
    }
}
