namespace GuessOrLose
{
    public readonly struct ValueContainer<TValue>
    {
        private readonly TValue? _value;

        public ValueContainer(TValue value)
        {
            _value = value;
            HasValue = true;
        }

        public ValueContainer()
        {
            _value = default;
            HasValue = false;
        }

        public TValue Value => HasValue ? _value! : throw new InvalidCastException();

        public bool HasValue { get; }

        public bool TryGetValue(out TValue? value)
        {
            if (HasValue)
            {
                value = Value;
                return true;
            }

            value = default;
            return false;
        }

        public override string ToString()
        {
            if (HasValue)
            {
                return Value?.ToString() ?? string.Empty;
            }
            else
            {
                return $$"""{ Empty value container of type {{typeof(TValue)}} }""";
            }
        }

        public static ValueContainer<TValue> Empty => new();

        public static implicit operator ValueContainer<TValue>(TValue value) => new(value);

        public static implicit operator TValue(ValueContainer<TValue> container) => container.Value;
    }
}
