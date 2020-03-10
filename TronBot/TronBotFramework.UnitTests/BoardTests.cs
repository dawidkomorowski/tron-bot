using System;
using System.Linq;
using NUnit.Framework;

namespace TronBotFramework.UnitTests
{
    public class BoardTests
    {
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(1, 0)]
        [TestCase(1, -1)]
        public void Constructor_ShouldThrowException_GivenNotPositiveDimensions(int width, int height)
        {
            // Arrange
            // Act
            // Assert
            Assert.That(() => new Board(width, height), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_ShouldCreateBoardOfSpecifiedSize()
        {
            // Arrange
            // Act
            var board = new Board(10, 20);

            // Assert
            Assert.That(board.Width, Is.EqualTo(10));
            Assert.That(board.Height, Is.EqualTo(20));
        }

        [Test]
        public void Constructor_ShouldCreateEmptyTronBoard()
        {
            // Arrange
            // Act
            var board = new Board(10, 20);

            // Assert
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 20; y++)
                {
                    var field = board.GetField(x, y);
                    Assert.That(field, Is.EqualTo(Field.Empty));
                }
            }
        }

        [TestCase(-1, 0)]
        [TestCase(10, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 20)]
        public void GetField_ShouldThrowException_GivenCoordinatesOutOfRange(int x, int y)
        {
            // Arrange
            var board = new Board(10, 20);

            // Act
            // Assert
            Assert.That(() => board.GetField(x, y), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1, 0)]
        [TestCase(10, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 20)]
        public void SetField_ShouldThrowException_GivenCoordinatesOutOfRange(int x, int y)
        {
            // Arrange
            var board = new Board(10, 20);

            // Act
            // Assert
            Assert.That(() => board.SetField(x, y, Field.Empty), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetField_ShouldReturnFieldThatWasSetWith_SetField()
        {
            // Arrange
            var board = new Board(10, 20);

            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 20; y++)
                {
                    // Act
                    var expectedField = GetRandomField();
                    board.SetField(x, y, expectedField);

                    // Assert
                    var field = board.GetField(x, y);
                    Assert.That(field, Is.EqualTo(expectedField));
                }
            }
        }

        [Test]
        public void GetEnumerator_ShouldReturnEnumeratorThatEnumeratesAllFields()
        {
            // Arrange
            var board = new Board(10, 20);

            // One blue head
            const int expectedBlueHead = 1;
            board.SetField(0, 0, Field.BlueHead);
            // One red head
            const int expectedRedHead = 1;
            board.SetField(9, 19, Field.RedHead);
            // Three blue tails
            const int expectedBlueTails = 3;
            board.SetField(1, 0, Field.BlueTail);
            board.SetField(1, 1, Field.BlueTail);
            board.SetField(1, 2, Field.BlueTail);
            // Five red tails
            const int expectedRedTails = 5;
            board.SetField(2, 0, Field.RedTail);
            board.SetField(2, 1, Field.RedTail);
            board.SetField(2, 2, Field.RedTail);
            board.SetField(2, 3, Field.RedTail);
            board.SetField(2, 4, Field.RedTail);
            // Two obstacles
            const int expectedObstacles = 2;
            board.SetField(3, 8, Field.Obstacle);
            board.SetField(3, 9, Field.Obstacle);
            // Rest is empty
            const int expectedEmpty = 10 * 20 - expectedBlueHead - expectedRedHead - expectedBlueTails - expectedRedTails - expectedObstacles;

            // Act
            var actualBlueHead = 0;
            var actualRedHead = 0;
            var actualBlueTails = 0;
            var actualRedTails = 0;
            var actualObstacles = 0;
            var actualEmpty = 0;
            foreach (var field in board)
            {
                switch (field)
                {
                    case Field.Empty:
                        actualEmpty++;
                        break;
                    case Field.Obstacle:
                        actualObstacles++;
                        break;
                    case Field.BlueTail:
                        actualBlueTails++;
                        break;
                    case Field.BlueHead:
                        actualBlueHead++;
                        break;
                    case Field.RedTail:
                        actualRedTails++;
                        break;
                    case Field.RedHead:
                        actualRedHead++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Assert
            Assert.That(board.Count(), Is.EqualTo(10 * 20));
            Assert.That(actualBlueHead, Is.EqualTo(expectedBlueHead));
            Assert.That(actualRedHead, Is.EqualTo(expectedRedHead));
            Assert.That(actualBlueTails, Is.EqualTo(expectedBlueTails));
            Assert.That(actualRedTails, Is.EqualTo(expectedRedTails));
            Assert.That(actualObstacles, Is.EqualTo(expectedObstacles));
            Assert.That(actualEmpty, Is.EqualTo(expectedEmpty));
        }

        private static Field GetRandomField()
        {
            return TestContext.CurrentContext.Random.NextEnum<Field>();
        }
    }
}