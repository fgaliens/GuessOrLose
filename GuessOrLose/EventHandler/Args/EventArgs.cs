namespace GuessOrLose.EventHandler.Args
{
    public abstract record EventArgs<TSource>
    {
        public required TSource Source { get; init; }
    }
}
