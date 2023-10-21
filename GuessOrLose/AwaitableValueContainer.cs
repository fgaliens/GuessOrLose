using System.Runtime.CompilerServices;

namespace GuessOrLose
{
    public readonly struct AwaitableValueContainer<TValue>
    {
        public AwaitableValueContainer(ValueContainer<TValue> value)
        {
            Value = new ValueTask<ValueContainer<TValue>>(value);
        }

        public AwaitableValueContainer(ValueTask<ValueContainer<TValue>> value)
        {
            Value = value;
        }

        public AwaitableValueContainer()
        {
            Value = new ValueTask<ValueContainer<TValue>>(ValueContainer<TValue>.Empty);
        }

        public ValueTask<ValueContainer<TValue>> Value { get; }

        public readonly ValueTaskAwaiter<ValueContainer<TValue>> GetAwaiter() => Value.GetAwaiter();

        public static AwaitableValueContainer<TValue> Empty => new();

    }

    public readonly struct AwaitableValueContainer
    {
        public static AwaitableValueContainer<TValue> FromValue<TValue>(TValue value)
        {
            return new AwaitableValueContainer<TValue>(value);
        }

        public static AwaitableValueContainer<TValue> FromExecutionContext<TValue>(Func<ValueTask<ValueContainer<TValue>>> context)
        {
            return new AwaitableValueContainer<TValue>(context());
        }
    }
}
