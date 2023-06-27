using MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener;
using MonoGameDrawingApp.VectorSprites.Export;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSpriteItemReference : IListenable
    {
        private readonly string _initPath;
        private readonly VectorSprite _initSprite;
        private bool _createFromPath;
        private bool _recursing;
        private VectorSpriteItem _item;

        public VectorSpriteItemReference() : this(null)
        {
        }

        public VectorSpriteItemReference(VectorSpriteItem item)
        {
            _item = item;
            _createFromPath = false;
        }

        public VectorSpriteItemReference(string path, VectorSprite sprite)
        {
            _initPath = path;
            _initSprite = sprite;
            _createFromPath = true;
        }

        public VectorSpriteItem Item
        {
            get
            {
                if (_createFromPath)
                {
                    try
                    {
                        LoadFromPath();
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
                if (_item == value)
                {
                    return;
                }

                if (_item != null)
                {
                    _item.GetAttachment<ChangeListenerVectorSpriteItemAttachment>().Changed -= CallChanged;
                }
                if (!_createFromPath)
                {
                    CallChanging();
                }
                _item = value;
                if (!_createFromPath)
                {
                    CallChanged();
                }
                if (_item != null)
                {
                    _item.GetAttachment<ChangeListenerVectorSpriteItemAttachment>().Changed += CallChanged;
                }
            }
        }

        public ModifiedGeometry ModifiedGeometry
        {
            get
            {
                if (_recursing)
                {
                    Item = null;
                    return null;
                }

                _recursing = true;

                ModifiedGeometry res = Item == null ? null : new ModifiedGeometry(Item.Geometry);

                _recursing = false;

                return res;
            }
        }

        public string Path
        {
            get
            {
                List<int> indices = new();
                VectorSpriteItem item = _item;
                if (item == null)
                {
                    return "NULL";
                }
                while (item.Parent != null)
                {
                    indices.Add(Array.IndexOf(item.Parent.Children.ToArray(), item));
                    item = item.Parent;
                }
                return string.Join("/", indices.Reverse<int>().ToArray());
            }
        }

        public Action Changing { get; set; }

        public Action Changed { get; set; }

        private void LoadFromPath()
        {
            if (_initPath == "NULL")
            {
                Item = null;
                _createFromPath = false;
                return;
            }
            VectorSpriteItem item = _initSprite.Root;
            IEnumerable<int> indices = _initPath.Split('/').Select(int.Parse);
            foreach (int i in indices)
            {
                item = item.GetChild(i);
            }
            Item = item;
            _createFromPath = false;
        }

        private void CallChanging()
        {
            Changing?.Invoke();
        }

        private void CallChanged()
        {
            if (!_recursing)
            {
                Changed?.Invoke();
            }
        }
    }
}
