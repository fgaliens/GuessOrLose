namespace GuessOrLose.Exceptions
{
    public class IncorrectOperationException : GuessOrLoseException
    {
        public IncorrectOperationException(ExceptionCode code, string message) : base(code, message)
        {
            Detail = "Incorrect operation";
        }
    }
}
