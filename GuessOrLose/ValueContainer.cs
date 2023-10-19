namespace GuessOrLose
{
    public readonly struct ValueContainer<TValue>
    {
        public ValueContainer(TValue value)
        {
            Value = value;
            HasValue = true;
        }

        public ValueContainer()
        {
            Value = default;
            HasValue = false;
        }

        public TValue? Value { get; }

        public bool HasValue { get; }

        public bool TryGetValue(out TValue? value)
        {
            value = Value;
            return HasValue;
        }

        public static ValueContainer<TValue> Empty => new();

        public static implicit operator ValueContainer<TValue>(TValue value) => new(value);

        public static explicit operator TValue(ValueContainer<TValue> container) => container.HasValue ? container.Value! : throw new InvalidCastException();
    }
}
