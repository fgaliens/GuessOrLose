namespace GuessOrLose.Exceptions
{
    public class IncorrectOperationException : GuessOrLoseException
    {
        public static IncorrectOperationException Create(string message)
        {
            return new IncorrectOperationException(message);
        }

        public IncorrectOperationException(string message) : base(ExceptionCode.Unspecified, message)
        { }
    }
}
