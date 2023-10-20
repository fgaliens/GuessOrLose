namespace GuessOrLose.Exceptions
{
    public enum ExceptionCode : int
    {
        Unspecified = 0,
        Forbidden = 1000,
        ActionForbiddenDueToState = 1001,
        PlayerNotInThisGame = 1002,
        NotFound = 2000,
        GameNotFound = 2001,
        PlayerNotFound = 2002,
        MessagesError = 3000,
        MessageIsNotValid = 3001,
        MessageWasNotWritten = 3002,
        Validation = 4000,
        IncorrectName = 4001
    }
}
