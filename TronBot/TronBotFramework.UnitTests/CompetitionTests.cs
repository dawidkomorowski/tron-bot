using NUnit.Framework;

namespace TronBotFramework.UnitTests
{
    public class CompetitionTests
    {
        #region Constructor tests

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

        #endregion

        [TestCase(1, 1)]
        [TestCase(5, 10)]
        public void BluePosition_ShouldReturnCorrectValueAfterConstruction(int blueX, int blueY)
        {
            // Arrange
            var board = GetValidBoard(bluePosition: (blueX, blueY));
            var competition = new Competition(board);

            // Act
            var bluePosition = competition.BluePosition;

            // Assert
            Assert.That(bluePosition, Is.EqualTo((blueX, blueY)));
        }

        [TestCase(8, 8)]
        [TestCase(5, 10)]
        public void RedPosition_ShouldReturnCorrectValueAfterConstruction(int redX, int redY)
        {
            // Arrange
            var board = GetValidBoard(redPosition: (redX, redY));
            var competition = new Competition(board);

            // Act
            var redPosition = competition.RedPosition;

            // Assert
            Assert.That(redPosition, Is.EqualTo((redX, redY)));
        }

        [TestCase(Color.Blue, true, true, true, true, new[] {Move.Up, Move.Down, Move.Left, Move.Right})]
        [TestCase(Color.Red, true, true, true, true, new[] {Move.Up, Move.Down, Move.Left, Move.Right})]
        [TestCase(Color.Blue, false, true, true, true, new[] {Move.Down, Move.Left, Move.Right})]
        [TestCase(Color.Red, false, true, true, true, new[] {Move.Down, Move.Left, Move.Right})]
        [TestCase(Color.Blue, true, false, true, true, new[] {Move.Up, Move.Left, Move.Right})]
        [TestCase(Color.Red, true, false, true, true, new[] {Move.Up, Move.Left, Move.Right})]
        [TestCase(Color.Blue, true, true, false, true, new[] {Move.Up, Move.Down, Move.Right})]
        [TestCase(Color.Red, true, true, false, true, new[] {Move.Up, Move.Down, Move.Right})]
        [TestCase(Color.Blue, true, true, true, false, new[] {Move.Up, Move.Down, Move.Left})]
        [TestCase(Color.Red, true, true, true, false, new[] {Move.Up, Move.Down, Move.Left})]
        [TestCase(Color.Blue, false, false, false, false, new Move[0])]
        [TestCase(Color.Red, false, false, false, false, new Move[0])]
        public void GetAvailableMoves_ShouldReturnMovesForFieldsThatAreEmpty(
            Color color, bool upIsEmpty, bool downIsEmpty, bool leftIsEmpty, bool rightIsEmpty, Move[] expectedMoves)
        {
            // Arrange
            var board = color == Color.Blue ? GetValidBoard(bluePosition: (5, 10)) : GetValidBoard(redPosition: (5, 10));
            board.SetField(5, 9, upIsEmpty ? Field.Empty : Field.Obstacle);
            board.SetField(5, 11, downIsEmpty ? Field.Empty : Field.Obstacle);
            board.SetField(4, 10, leftIsEmpty ? Field.Empty : Field.Obstacle);
            board.SetField(6, 10, rightIsEmpty ? Field.Empty : Field.Obstacle);

            var competition = new Competition(board);

            // Act
            var availableMoves = competition.GetAvailableMoves(color);

            // Assert
            Assert.That(availableMoves, Is.EquivalentTo(expectedMoves));
        }

        private static Board GetValidBoard((int X, int Y)? bluePosition = null, (int X, int Y)? redPosition = null)
        {
            var board = new Board(10, 20);

            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 20; y++)
                {
                    board.SetField(x, y, Field.Obstacle);
                }
            }

            board.SetField(bluePosition?.X ?? 1, bluePosition?.Y ?? 1, Field.BlueHead);
            board.SetField(redPosition?.X ?? 8, redPosition?.Y ?? 8, Field.RedHead);

            return board;
        }
    }
}