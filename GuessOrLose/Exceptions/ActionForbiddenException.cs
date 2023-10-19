namespace GuessOrLose.Exceptions
{
    public class ActionForbiddenException : GuessOrLoseException
    {
        public ActionForbiddenException(ExceptionCode code, string reason)
            : base(code, $"Action is forbidden: {reason}")
        {
            Detail = "This action can't be performed";
        }
    }
}
