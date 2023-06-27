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
        private bool _isVisible;

        public VectorSpriteItem(string name, VectorSprite sprite, VectorSpriteGeometry geometry, Vector2 position)
        {
            _children = new List<VectorSpriteItem>();
            _modifiers = new List<IGeometryModifier>();
            _attchments = new Dictionary<Type, IVectorSpriteItemAttachment>();
            _isVisible = true;

            Sprite = sprite;
            Name = name;
            Geometry = geometry;
            Geometry.Item = this;
            Position = position;

            Sprite.ApplyAttachments(this);
        }

        public VectorSpriteItem(string name, VectorSprite sprite) : this(name, sprite, new VectorSpriteGeometry(), Vector2.Zero)
        {
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    DataChanging();
                    _isVisible = value;
                    DataChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    DataChanging();
                    _name = value;
                    DataChanged();
                }
            }
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    DataChanging();
                    _position = value;
                    DataChanged();
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
            set => Position = value - (AbsolutePosition - _position);
        }

        public VectorSprite Sprite { get; }

        public VectorSpriteGeometry Geometry { get; }

        public VectorSpriteItem Parent { get; private set; }


        public IEnumerable<VectorSpriteItem> Children
        {
            get => _children;
            set
            {
                ChildrenChanging();
                _children.Clear();
                _children.Capacity = value.Count();
                foreach (VectorSpriteItem child in value)
                {
                    child.Parent?.RemoveChild(child);
                    _children.Add(child);
                    child.Parent = this;
                }
                ChildrenChanged();
            }
        }

        public IEnumerable<IGeometryModifier> Modifiers
        {
            get => _modifiers;
            set
            {
                DataChanging();
                foreach (IGeometryModifier modifier in _modifiers.ToArray())
                {
                    ForceRemoveModifier(modifier);
                }
                foreach (IGeometryModifier modifier in value.ToArray())
                {
                    ForceAddModifier(modifier);
                }
                DataChanged();
            }
        }

        public void AddAttachment(IVectorSpriteItemAttachment attachment)
        {
            _attchments.TryAdd(attachment.GetType(), attachment);
        }

        public T GetAttachment<T>() where T : IVectorSpriteItemAttachment
        {
            return (T)_attchments[typeof(T)];
        }

        public VectorSpriteItem GetChild(int index)
        {
            return _children[index];
        }

        public void RemoveChild(VectorSpriteItem child)
        {
            ChildrenChanging();
            _children.Remove(child);
            child.Parent = null;
            ChildrenChanged();
        }

        public void AddChild(VectorSpriteItem child)
        {
            InsertChild(_children.Count, child);
        }

        public void InsertChild(int index, VectorSpriteItem child)
        {
            ChildrenChanging();
            child.Parent?.RemoveChild(child);
            _children.Insert(index, child);
            child.Parent = this;
            ChildrenChanged();
        }

        public void AddModifier(IGeometryModifier modifier)
        {
            DataChanging();
            ForceAddModifier(modifier);
            DataChanged();
        }

        public void RemoveModifier(IGeometryModifier modifier)
        {
            DataChanging();
            ForceRemoveModifier(modifier);
            DataChanged();
        }

        public void MoveModifierUp(IGeometryModifier modifier)
        {
            DataChanging();
            int index = _modifiers.IndexOf(modifier);
            if (index > 0)
            {
                _modifiers.RemoveAt(index);
                _modifiers.Insert(index - 1, modifier);
            }
            DataChanged();
        }
        public void MoveModifierDown(IGeometryModifier modifier)
        {
            DataChanging();
            int index = _modifiers.IndexOf(modifier);
            if (index < _modifiers.Count - 1)
            {
                _modifiers.RemoveAt(index);
                _modifiers.Insert(index + 1, modifier);
            }
            DataChanged();
        }

        public void MoveUp()
        {
            List<VectorSpriteItem> children = Parent._children;
            int index = children.IndexOf(this);
            if (index > 0)
            {
                Parent.ChildrenChanging();
                children.RemoveAt(index);
                children.Insert(index - 1, this);
                Parent.ChildrenChanged();
            }
        }

        public void MoveDown()
        {
            List<VectorSpriteItem> children = Parent._children;
            int index = children.IndexOf(this);
            if (index < children.Count - 1)
            {
                Parent.ChildrenChanging();
                children.RemoveAt(index);
                children.Insert(index + 1, this);
                Parent.ChildrenChanged();
            }
        }

        private void ForceRemoveModifier(IGeometryModifier modifier)
        {
            _modifiers.Remove(modifier);
            foreach (IGeometryModifierParameter parameter in modifier.Parameters)
            {
                parameter.Changed -= DataChanged;
                parameter.Changing -= DataChanging;
            }
        }

        private void ForceAddModifier(IGeometryModifier modifier)
        {
            _modifiers.Add(modifier);
            foreach (IGeometryModifierParameter parameter in modifier.Parameters)
            {
                parameter.Changed += DataChanged;
                parameter.Changing += DataChanging;
            }
        }

        private void ChildrenChanged()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.ChildrenChanged();
            }
        }

        internal void DataChanged()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.DataChanged();
            }
        }

        private void ChildrenChanging()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.ChildrenChanging();
            }
        }

        internal void DataChanging()
        {
            foreach (IVectorSpriteItemAttachment attachment in _attchments.Values)
            {
                attachment.DataChanging();
            }
        }
    }
}
