using NUnit.Framework;

namespace TronBotFramework.UnitTests
{
    public class CompetitionTests
    {
        [Test]
        public void Constructor_ShouldNotThrow_GivenValidBoard()
        {
            // Arrange
            var board = GetValidBoard();

            // Act
            // Assert
            Assert.That(() => new Competition(board), Throws.Nothing);
        }

        [Test]
        public void Constructor_ShouldThrowException_GivenBoardWithMissingBlueHead()
        {
            // Arrange
            var board = GetValidBoard();
            board.SetField(1, 1, Field.Empty);

            // Act
            // Assert
            Assert.That(() => new Competition(board), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_ShouldThrowException_GivenBoardWithMissingRedHead()
        {
            // Arrange
            var board = GetValidBoard();
            board.SetField(8, 8, Field.Empty);

            // Act
            // Assert
            Assert.That(() => new Competition(board), Throws.ArgumentException);
        }

        [TestCase(5, 0)]
        [TestCase(5, 19)]
        [TestCase(0, 10)]
        [TestCase(9, 10)]
        public void Constructor_ShouldThrowException_GivenBoardWithBorderMissingAnObstacle(int x, int y)
        {
            // Arrange
            var board = GetValidBoard();
            board.SetField(x, y, Field.Empty);

            // Act
            // Assert
            Assert.That(() => new Competition(board), Throws.ArgumentException);
        }

        private static Board GetValidBoard()
        {
            var board = new Board(10, 20);

            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 20; y++)
                {
                    board.SetField(x, y, Field.Obstacle);
                }
            }

            board.SetField(1, 1, Field.BlueHead);
            board.SetField(8, 8, Field.RedHead);

            return board;
        }
    }
}