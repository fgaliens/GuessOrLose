namespace GuessOrLose.Exceptions
{
    public class ObjectNotFoundException : GuessOrLoseException
    {
        public ObjectNotFoundException(ExceptionCode code, Type storageType) 
            : base(code, $"Object of type {storageType} wasn`t found")
        {
            Detail = "Object wasn`t found";
        }

        public ObjectNotFoundException(Type storageType) 
            : this(ExceptionCode.NotFound, storageType)
        { }
    }
}
