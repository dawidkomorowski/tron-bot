using TronBotFramework;

namespace MonteusCarlus
{
    internal readonly struct ScoredMove
    {
        public ScoredMove(Move move, int won, int played)
        {
            Move = move;
            Won = won;
            Played = played;
        }

        public Move Move { get; }
        public int Won { get; }
        public int Played { get; }
    }
}