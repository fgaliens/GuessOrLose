namespace GuessOrLose.Exceptions
{
    public class ValidationException : GuessOrLoseException
    {
        public ValidationException(ExceptionCode code, string message) : base(code, message)
        {
            Detail = "Validation error";
        }
    }
}
