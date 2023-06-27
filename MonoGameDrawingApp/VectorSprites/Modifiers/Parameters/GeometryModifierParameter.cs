using System;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Parameters
{
    public sealed class GeometryModifierParameter<T> : IGeometryModifierParameter, IListenable
    {
        private T _value;
        private readonly Func<T, T> _change;

        public GeometryModifierParameter(T value, string name, Func<T, T> change = null)
        {
            SetValue(value);
            _change = change;
            Name = name;
        }

        public string Name { get; init; }

        public Action Changing { get; set; }

        public Action Changed { get; set; }

        public T Value
        {
            get => _value;
            set
            {
                T val = _change == null ? value : _change.Invoke(value);
                if (!Equals(_value, val))
                {
                    if (_value is IListenable listenable)
                    {
                        listenable.Changing -= OnChanging;
                        listenable.Changed -= OnChanged;
                    }
                    OnChanging();
                    SetValue(val);
                    OnChanged();
                }
            }
        }

        private void SetValue(T val)
        {
            _value = val;
            if (val is IListenable listenable)
            {
                listenable.Changing += OnChanging;
                listenable.Changed += OnChanged;
            }
        }

        private void OnChanging()
        {
            Changing?.Invoke();
        }

        private void OnChanged()
        {
            Changed?.Invoke();
        }

        public object ObjectValue => _value;
    }
}
