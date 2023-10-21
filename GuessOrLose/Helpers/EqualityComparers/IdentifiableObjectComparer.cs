using System.Diagnostics.CodeAnalysis;

namespace GuessOrLose.Helpers.EqualityComparers
{
    public class IdentifiableObjectComparer<T> : IEqualityComparer<T> where T : IIdentifiable
    {
        public IdentifiableObjectComparer()
        {
            Enable = typeof(IIdentifiable).IsAssignableFrom(typeof(T));
        }

        public bool Enable { get; }

        public bool Equals(T? x, T? y)
        {
            ThrowIfNotAvailable();

            return x?.Id == y?.Id;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.Id.GetHashCode();
        }

        private void ThrowIfNotAvailable()
        {
            if (!Enable)
            {
                throw new NotSupportedException($"Equality comparison for type {typeof(T)} is not available");
            }
        }
    }
}
