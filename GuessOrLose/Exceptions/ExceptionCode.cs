﻿namespace GuessOrLose.Exceptions
{
    public enum ExceptionCode : int
    {
        Unspecified = 0,
        Forbidden = 1000,
        ActionForbiddenDueToState = 1001,
        PlayerNotInThisGame = 1002,
        NotFound = 2000,
        MessagesError = 3000,
        MessageIsNotValid = 3001,
        MessageWasNotWritten = 3002,
    }
}