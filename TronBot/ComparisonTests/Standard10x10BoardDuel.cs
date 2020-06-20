using NUnit.Framework;
using TronBotFramework;

namespace ComparisonTests
{
    [TestFixture]
    public sealed class Standard10x10BoardDuel
    {
        private const int Iterations = 100;
        private Board _scenarioBoard = null!;

        [SetUp]
        public void SetUp()
        {
            _scenarioBoard = new Board(10, 10);

            for (var x = 0; x < _scenarioBoard.Width; x++)
            {
                for (var y = 0; y < _scenarioBoard.Height; y++)
                {
                    if (x > 0 && x < _scenarioBoard.Width - 1 && y > 0 && y < _scenarioBoard.Height - 1)
                    {
                        _scenarioBoard.SetField(x, y, Field.Empty);
                    }
                    else
                    {
                        _scenarioBoard.SetField(x, y, Field.Obstacle);
                    }
                }
            }

            _scenarioBoard.SetField(1, 1, Field.BlueHead);
            _scenarioBoard.SetField(8, 8, Field.RedHead);
        }

        [Test]
        public void MinimusMaximus_WinsAgainst_RandBot_AtLeast50PercentTimes()
        {
            var minimusMaximus = new MinimusMaximus.TronBot();
            var randBot = new RandBot.TronBot();

            var statistics = Duel.RunMultiple(minimusMaximus, randBot, _scenarioBoard, Iterations);

            const int acceptanceThreshold = (int) (0.5 * Iterations);
            Assert.That(statistics.BlueWon, Is.GreaterThan(acceptanceThreshold));
        }
    }
}