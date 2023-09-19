namespace GuessOrLose.Models
{
    public record Player(Guid Id)
    {
        public string  Name { get; set; } = string.Empty;
    }
}
