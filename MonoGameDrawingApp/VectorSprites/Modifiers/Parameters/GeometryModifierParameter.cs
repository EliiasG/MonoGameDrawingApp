using System;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Parameters
{
    public sealed class GeometryModifierParameter<T> : IGeometryModifierParameter, IListenable
    {
        private T _value;
        private readonly Func<T, T> _change;

        public GeometryModifierParameter(T value, string name, Func<T, T> change = null)
        {
            _setValue(value);
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
                        listenable.Changing -= _changing;
                        listenable.Changed -= _changed;
                    }
                    _changing();
                    _setValue(val);
                    _changed();
                }
            }
        }

        private void _setValue(T val)
        {
            _value = val;
            if (val is IListenable listenable)
            {
                listenable.Changing += _changing;
                listenable.Changed += _changed;
            }
        }

        private void _changing() => Changing?.Invoke();
        private void _changed() => Changed?.Invoke();

        public object ObjectValue => _value;
    }
}
