using TronBotFramework;
using TronBotFramework.Interface.Version1;

namespace RandBot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var runner = new TronBotRunner(new TronBot(), new TronBotInterface());
            runner.Run();
        }
    }
}