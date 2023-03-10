using MonoGameDrawingApp.VectorSprites.Attachments;
using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSpriteItem
    {
        private readonly List<VectorSpriteItem> _children;
        private readonly List<IGeometryModifier> _modifiers;
        private readonly Dictionary<Type, IVectorSpriteItemAttachment> _attchments;

        private string _name;
        private Vector2 _position;

        public VectorSpriteItem(string name, VectorSprite sprite, VectorSpriteGeometry geometry, Vector2 position)
        {
            _children = new List<VectorSpriteItem>();
            _modifiers = new List<IGeometryModifier>();
            _attchments = new Dictionary<Type, IVectorSpriteItemAttachment>();

            Sprite = sprite;
            Name = name;
            Geometry = geometry;
            Geometry.VectorSpriteItem = this;
            Position = position;

            Sprite.ApplyAttachments(this);
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

        public Vector2 AbsolutePosition
        {
            get
            {
                VectorSpriteItem parent = this;
                Vector2 position = Vector2.Zero;
                while (parent != null)
                {
                    position += parent.Position;
                    parent = parent.Parent;
                }
                return position;
            }
            set
            {
                Position = value - (AbsolutePosition - _position);
            }
        }

        public VectorSprite Sprite { get; init; }

        public VectorSpriteGeometry Geometry { get; init; }

        public VectorSpriteItem Parent { get; private set; }


        public IEnumerable<VectorSpriteItem> Children
        {
            get => _children;
            set
            {
                _childrenChanging();
                _children.Clear();
                _children.Capacity = value.Count();
                foreach (VectorSpriteItem child in value)
                {
                    child.Parent?.RemoveChild(child);
                    _children.Add(child);
                    child.Parent = this;
                }
                _childrenChanged();
            }
        }

        public IEnumerable<IGeometryModifier> Modifiers
        {
            get => _modifiers;
            set
            {
                _dataChanging();
                foreach (IGeometryModifier modifier in _modifiers.ToArray())
                {
                    _removeModifier(modifier);
                }
                foreach (IGeometryModifier modifier in value)
                {
                    _addModifier(modifier);
                }
                _dataChanged();
            }
        }

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

        public void AddModifier(IGeometryModifier modifier)
        {
            _dataChanging();
            _addModifier(modifier);
            _dataChanged();
        }

        public void RemoveModifier(IGeometryModifier modifier)
        {
            _dataChanging();
            _removeModifier(modifier);
            _dataChanged();
        }

        public void MoveModifierUp(IGeometryModifier modifier)
        {
            _dataChanging();
            int index = _modifiers.IndexOf(modifier);
            if (index > 0)
            {
                _modifiers.RemoveAt(index);
                _modifiers.Insert(index - 1, modifier);
            }
            _dataChanged();
        }
        public void MoveModifierDown(IGeometryModifier modifier)
        {
            _dataChanging();
            int index = _modifiers.IndexOf(modifier);
            if (index < _modifiers.Count - 1)
            {
                _modifiers.RemoveAt(index);
                _modifiers.Insert(index + 1, modifier);
            }
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

        private void _removeModifier(IGeometryModifier modifier)
        {
            _modifiers.Remove(modifier);
            foreach (IGeometryModifierParameter parameter in modifier.Parameters)
            {
                parameter.Changed -= _dataChanged;
                parameter.Changing -= _dataChanging;
            }
        }

        private void _addModifier(IGeometryModifier modifier)
        {
            _modifiers.Add(modifier);
            foreach (IGeometryModifierParameter parameter in modifier.Parameters)
            {
                parameter.Changed += _dataChanged;
                parameter.Changing += _dataChanging;
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
