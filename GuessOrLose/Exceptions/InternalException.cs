namespace GuessOrLose.Exceptions
{
    public class InternalException : GuessOrLoseException
    {
        public InternalException(ExceptionCode code, string message) : base(code, message)
        {
            Detail = "Some internal exception has occurred";
        }

        public InternalException(ExceptionCode code, string message, Exception exception) : base(code, message, exception)
        {
            Detail = "Some internal exception has occurred";
        }
    }
}
