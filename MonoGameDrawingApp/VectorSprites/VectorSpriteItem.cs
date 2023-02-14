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
                _name = value;
                _dataChanged();
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
                _position = value;
                _dataChanged();
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
            _children.Remove(child);
            child.Parent = null;
            _childrenChanged();
        }

        public void AddChild(VectorSpriteItem child)
        {
            child.Parent?.RemoveChild(child);
            _children.Add(child);
            child.Parent = this;
            _childrenChanged();
        }

        public void AddModifier(IVectorSpriteItemModifier modifier)
        {
            _modifiers.Add(modifier);
            _dataChanged();
        }

        public void RemoveModifier(IVectorSpriteItemModifier modifier)
        {
            _modifiers.Remove(modifier);
            _dataChanged();
        }

        public void MoveUp()
        {
            List<VectorSpriteItem> children = Parent._children;
            int index = children.IndexOf(this);
            if (index > 0)
            {
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
                children.RemoveAt(index);
                children.Insert(index + 1, this);
                Parent._childrenChanged();
            }
        }

        internal void _childrenChanged()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.ChildrenChanged();
            }
        }

        private void _dataChanged()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.DataChanged();
            }
        }
    }
}
