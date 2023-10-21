using GuessOrLose.GameServices;
using GuessOrLose.GameServices.Stages;
using GuessOrLose.Messages;
using GuessOrLose.Players;

namespace GuessOrLose.Models.Messages
{
    public static class JoinPlayersStageMessages
    {
        public record StateChanged : Message<JoinPlayersStage>
        {
            public StageState PreviousState { get; set; }
            public StageState CurrentState { get; set; }

            public override bool IsValid()
            {
                return true;
            }
        }

        public record PlayerJoined : Message<JoinPlayersStage>
        {
            public Player Player { get; set; }

            public override bool IsValid()
            {
                return Player is not null;
            }
        }

        public record PlayerReadyToStart : Message<JoinPlayersStage>
        {
            public Player Player { get; set; }

            public override bool IsValid()
            {
                return Player is not null;
            }
        }
    }

}
