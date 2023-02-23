using MonoGameDrawingApp.VectorSprites.Attachments;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSpriteItem
    {
        private readonly List<VectorSpriteItem> _children;
        private readonly List<IVectorSpriteItemModifier> _modifiers;
        private readonly Dictionary<Type, IVectorSpriteItemAttachment> _attchments;

        private string _name;
        private Vector2 _position;

        public VectorSpriteItem(string name, VectorSprite sprite, VectorSpriteGeometry geometry, Vector2 position)
        {
            _children = new List<VectorSpriteItem>();
            _modifiers = new List<IVectorSpriteItemModifier>();
            _attchments = new Dictionary<Type, IVectorSpriteItemAttachment>();
            Sprite = sprite;
            Name = name;
            Sprite.ApplyAttachments(this);
            Geometry = geometry;
            Geometry.VectorSpriteItem = this;
            Position = position;
        }

        public VectorSpriteItem(string name, VectorSprite sprite) : this(name, sprite, new VectorSpriteGeometry(), Vector2.Zero)
        {
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _dataChanging();
                    _name = value;
                    _dataChanged();
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (_position != value)
                {
                    _dataChanging();
                    _position = value;
                    _dataChanged();
                }
            }
        }

        public VectorSprite Sprite { get; init; }

        public VectorSpriteGeometry Geometry { get; init; }

        public VectorSpriteItem Parent { get; private set; }

        public IEnumerable<VectorSpriteItem> Children => _children;

        public IEnumerable<IVectorSpriteItemModifier> Modifiers => _modifiers;

        public void AddAttachment(IVectorSpriteItemAttachment attachment)
        {
            _attchments.TryAdd(attachment.GetType(), attachment);
        }

        public Type GetAttachment<Type>() where Type : IVectorSpriteItemAttachment
        {
            return (Type)_attchments[typeof(Type)];
        }

        public void RemoveChild(VectorSpriteItem child)
        {
            _childrenChanging();
            _children.Remove(child);
            child.Parent = null;
            _childrenChanged();
        }

        public void AddChild(VectorSpriteItem child)
        {
            _childrenChanging();
            child.Parent?.RemoveChild(child);
            _children.Add(child);
            child.Parent = this;
            _childrenChanged();
        }

        public void AddModifier(IVectorSpriteItemModifier modifier)
        {
            _dataChanging();
            _modifiers.Add(modifier);
            _dataChanged();
        }

        public void RemoveModifier(IVectorSpriteItemModifier modifier)
        {
            _dataChanging();
            _modifiers.Remove(modifier);
            _dataChanged();
        }

        public void MoveUp()
        {
            List<VectorSpriteItem> children = Parent._children;
            int index = children.IndexOf(this);
            if (index > 0)
            {
                Parent._childrenChanging();
                children.RemoveAt(index);
                children.Insert(index - 1, this);
                Parent._childrenChanged();
            }
        }

        public void MoveDown()
        {
            List<VectorSpriteItem> children = Parent._children;
            int index = children.IndexOf(this);
            if (index < children.Count - 1)
            {
                Parent._childrenChanging();
                children.RemoveAt(index);
                children.Insert(index + 1, this);
                Parent._childrenChanged();
            }
        }

        private void _childrenChanged()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.ChildrenChanged();
            }
        }

        internal void _dataChanged()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.DataChanged();
            }
        }

        private void _childrenChanging()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.ChildrenChanging();
            }
        }

        internal void _dataChanging()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.DataChanging();
            }
        }
    }
}
