namespace TronBotFramework
{
    public interface IBot
    {
        Move FindMove(Board board, Color color);
    }
}