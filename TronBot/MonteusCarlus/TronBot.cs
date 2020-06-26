using TronBotFramework;

namespace MonteusCarlus
{
    public sealed class TronBot : IBot
    {
        public Move FindMove(Board board, Color color)
        {
            return Move.Up;
        }
    }
}