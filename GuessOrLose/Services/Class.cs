using GuessOrLose.Models;

namespace GuessOrLose.Test
{
    public class TeamTurn
    {

    }

    public class Round
    {
        private readonly Game game;

        public Round(Game game)
        {
            this.game = game;
        }

        public TeamTurn NextTurn()
        {

        }
    }

    public class Game
    {
        public Game()
        {
            
        }

        public Round StartRound()
        {
            return new(this);
        }
    }


}
