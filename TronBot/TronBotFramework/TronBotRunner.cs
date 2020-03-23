using System;

namespace TronBotFramework
{
    public sealed class TronBotRunner
    {
        private readonly IBot _bot;
        private readonly IInterface _interface;

        public TronBotRunner(IBot bot, IInterface @interface)
        {
            _bot = bot;
            _interface = @interface;
        }

        public void Run()
        {
            Response response;

            do
            {
                var command = Console.ReadLine();
                response = _interface.ProcessCommand(command, _bot);
                Console.WriteLine(response.Content);
            } while (response.WaitForNextCommand);
        }
    }
}