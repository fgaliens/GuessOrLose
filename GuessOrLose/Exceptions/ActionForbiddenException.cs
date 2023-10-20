namespace GuessOrLose.Exceptions
{
    public class ActionForbiddenException : GuessOrLoseException, IGuessOrLoseException<ActionForbiddenException>
    {
        public ActionForbiddenException(ExceptionCode code, string reason)
            : base(code, $"Action is forbidden: {reason}")
        {
            Detail = "This action can't be performed";
        }

        public static ActionForbiddenException Create(ExceptionCode exceptionCode, string message)
        {
            return new ActionForbiddenException(exceptionCode, message);
        }
    }
}
