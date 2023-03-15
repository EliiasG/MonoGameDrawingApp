using System;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Parameters
{
    public sealed class GeometryModifierParameter<T> : IGeometryModifierParameter
    {
        private T _value;
        private readonly Func<T, T> _change;

        public GeometryModifierParameter(T value, string name, Func<T, T> change = null)
        {
            _value = value;
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
                if (!object.Equals(_value, val))
                {
                    Changing?.Invoke();
                    _value = val;
                    Changed?.Invoke();
                }
            }
        }

        public object ObjectValue => _value;
    }
}
