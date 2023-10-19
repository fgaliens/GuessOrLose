namespace GuessOrLose.Messages
{
    public abstract record Message
    {
        public abstract object SourceObj { get; }
        public abstract bool IsValid();
    }

    public abstract record Message<TSource> : Message
    {
        public TSource Source { get; set; }
        public override object SourceObj => Source!;
    }
}
