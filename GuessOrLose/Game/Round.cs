using GuessOrLose.Exceptions;

namespace GuessOrLose.Game
{
    public class Round
    {
        private readonly _Game game;
        private readonly WordsDeck wordsDeck;

        public bool Finished => WordsDeck.IsEmpty;

        public WordsDeck WordsDeck => wordsDeck;

        public Round(_Game game)
        {
            this.game = game;
            this.wordsDeck = new(game.Words, game.OrderRandomizer);
        }

        public TeamTurn NextTurn()
        {
            if (Finished)
            {
                throw new IncorrectOperationException("Word's deck is empty");
            }

            var team = game.Teams.Next();
            return new(team);
        }
    }
}
