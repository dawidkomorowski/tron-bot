namespace TronBotFramework
{
    public interface IInterface
    {
        Response ProcessCommand(string command, IBot bot);
    }
}