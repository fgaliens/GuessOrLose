namespace GuessOrLose.Exceptions
{
    public class GuessOrLoseException : Exception
    {
        public GuessOrLoseException(ExceptionCode exceptionCode)
        {
            Code = exceptionCode;
        }

        public GuessOrLoseException(ExceptionCode exceptionCode, string message) : base(message) 
        { 
            Code = exceptionCode; 
        }
        public GuessOrLoseException(ExceptionCode exceptionCode, string message, Exception inner) : base(message, inner) 
        { 
            Code = exceptionCode; 
        }

        public ExceptionCode Code { get; init; }
        public string Detail {  get; init; } = string.Empty;
    }
}
