namespace Cornershop.Service.Common
{
    public readonly struct Result<T, E> {
        public readonly bool Success;
        public readonly T? Value;
        public readonly E? Error;

        private Result(T v, E e, bool success)
        {
            Value = v;
            Error = e;
            Success = success;
        }

        public static implicit operator Result<T, E?>(T v)
        {
            return new(v, default, true);
        }

        public static implicit operator Result<T?, E>(E e)
        {
            return new(default, e, false);
        }
    }

}