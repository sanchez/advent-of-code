using System;

namespace Sanchez.Match
{
    public class MissingValueException : Exception
    {
        public MissingValueException() : base("Optional does not have a value") { }
    }

    public class Optional<T>
    {
        protected T _value;
        protected bool _hasValue;

        protected Optional(T val)
        {
            _value = val;
            _hasValue = true;
        }

        protected Optional()
        {
            _hasValue = false;
        }

        public static Optional<T> Some(T val) => new(val);
        public static Optional<T> None() => new();

        public bool HasValue => _hasValue;
        public T Value
        {
            get
            {
                if (!_hasValue) throw new MissingValueException();
                return _value;
            }
        }
        public bool TryGetValue(out T val)
        {
            val = _value;
            return _hasValue;
        }
    }
}
