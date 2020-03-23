namespace TronBotFramework
{
    public sealed class Response
    {
        public Response(string content, bool waitForNextCommand)
        {
            Content = content;
            WaitForNextCommand = waitForNextCommand;
        }

        public string Content { get; }
        public bool WaitForNextCommand { get; }
    }
}