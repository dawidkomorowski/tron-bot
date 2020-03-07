using NUnit.Framework;

namespace TronBotFramework.UnitTests
{
    public class TronCompetitionTests
    {
        [Test]
        public void Constructor_ShouldNotThrow_GivenValidTronBoard()
        {
            // Arrange
            var tronBoard = GetValidTronBoard();

            // Act
            // Assert
            Assert.That(() => new TronCompetition(tronBoard), Throws.Nothing);
        }

        [Test]
        public void Constructor_ShouldThrowException_GivenBoardWithMissingBlueHead()
        {
            // Arrange
            var tronBoard = GetValidTronBoard();
            tronBoard.SetField(1, 1, TronBoardField.Empty);

            // Act
            // Assert
            Assert.That(() => new TronCompetition(tronBoard), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_ShouldThrowException_GivenBoardWithMissingRedHead()
        {
            // Arrange
            var tronBoard = GetValidTronBoard();
            tronBoard.SetField(8, 8, TronBoardField.Empty);

            // Act
            // Assert
            Assert.That(() => new TronCompetition(tronBoard), Throws.ArgumentException);
        }

        [TestCase(5, 0)]
        [TestCase(5, 19)]
        [TestCase(0, 10)]
        [TestCase(9, 10)]
        public void Constructor_ShouldThrowException_GivenBoardWithBorderMissingAnObstacle(int x, int y)
        {
            // Arrange
            var tronBoard = GetValidTronBoard();
            tronBoard.SetField(x, y, TronBoardField.Empty);

            // Act
            // Assert
            Assert.That(() => new TronCompetition(tronBoard), Throws.ArgumentException);
        }

        private static TronBoard GetValidTronBoard()
        {
            var tronBoard = new TronBoard(10, 20);

            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 20; y++)
                {
                    tronBoard.SetField(x, y, TronBoardField.Obstacle);
                }
            }

            tronBoard.SetField(1, 1, TronBoardField.BlueHead);
            tronBoard.SetField(8, 8, TronBoardField.RedHead);

            return tronBoard;
        }
    }
}