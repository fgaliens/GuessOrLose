namespace GuessOrLose.Exceptions
{
    public class ObjectNotFoundException : GuessOrLoseException
    {
        public ObjectNotFoundException(ExceptionCode code, string message) : base(code, message)
        {
            Detail = "Object wasn`t found";
        }

        public ObjectNotFoundException(ExceptionCode code, object key) 
            : base(code, $"Object can't be found by key {key} ({key.GetType()})")
        {
            Detail = "Object wasn`t found";
        }

        public ObjectNotFoundException(ExceptionCode code, object key, string source)
            : base(code, $"Object can't be found by key {key} ({key.GetType()}) in {source}")
        {
            Detail = "Object wasn`t found";
        }
    }
}
