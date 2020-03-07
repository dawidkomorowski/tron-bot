using System;
using System.Linq;
using NUnit.Framework;

namespace TronBotFramework.UnitTests
{
    public class TronBoardTests
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
            Assert.That(() => new TronBoard(width, height), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_ShouldCreateTronBoardOfSpecifiedSize()
        {
            // Arrange
            // Act
            var tronBoard = new TronBoard(10, 20);

            // Assert
            Assert.That(tronBoard.Width, Is.EqualTo(10));
            Assert.That(tronBoard.Height, Is.EqualTo(20));
        }

        [Test]
        public void Constructor_ShouldCreateEmptyTronBoard()
        {
            // Arrange
            // Act
            var tronBoard = new TronBoard(10, 20);

            // Assert
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 20; y++)
                {
                    var field = tronBoard.GetField(x, y);
                    Assert.That(field, Is.EqualTo(TronBoardField.Empty));
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
            var tronBoard = new TronBoard(10, 20);

            // Act
            // Assert
            Assert.That(() => tronBoard.GetField(x, y), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1, 0)]
        [TestCase(10, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 20)]
        public void SetField_ShouldThrowException_GivenCoordinatesOutOfRange(int x, int y)
        {
            // Arrange
            var tronBoard = new TronBoard(10, 20);

            // Act
            // Assert
            Assert.That(() => tronBoard.SetField(x, y, TronBoardField.Empty), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetField_ShouldReturnFieldThatWasSetWith_SetField()
        {
            // Arrange
            var tronBoard = new TronBoard(10, 20);

            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 20; y++)
                {
                    // Act
                    var expectedField = GetRandomField();
                    tronBoard.SetField(x, y, expectedField);

                    // Assert
                    var field = tronBoard.GetField(x, y);
                    Assert.That(field, Is.EqualTo(expectedField));
                }
            }
        }

        [Test]
        public void GetEnumerator_ShouldReturnEnumeratorThatEnumeratesAllFields()
        {
            // Arrange
            var tronBoard = new TronBoard(10, 20);

            // One blue head
            const int expectedBlueHead = 1;
            tronBoard.SetField(0, 0, TronBoardField.BlueHead);
            // One red head
            const int expectedRedHead = 1;
            tronBoard.SetField(9, 19, TronBoardField.RedHead);
            // Three blue tails
            const int expectedBlueTails = 3;
            tronBoard.SetField(1, 0, TronBoardField.BlueTail);
            tronBoard.SetField(1, 1, TronBoardField.BlueTail);
            tronBoard.SetField(1, 2, TronBoardField.BlueTail);
            // Five red tails
            const int expectedRedTails = 5;
            tronBoard.SetField(2, 0, TronBoardField.RedTail);
            tronBoard.SetField(2, 1, TronBoardField.RedTail);
            tronBoard.SetField(2, 2, TronBoardField.RedTail);
            tronBoard.SetField(2, 3, TronBoardField.RedTail);
            tronBoard.SetField(2, 4, TronBoardField.RedTail);
            // Two obstacles
            const int expectedObstacles = 2;
            tronBoard.SetField(3, 8, TronBoardField.Obstacle);
            tronBoard.SetField(3, 9, TronBoardField.Obstacle);
            // Rest is empty
            const int expectedEmpty = 10 * 20 - expectedBlueHead - expectedRedHead - expectedBlueTails - expectedRedTails - expectedObstacles;

            // Act
            var actualBlueHead = 0;
            var actualRedHead = 0;
            var actualBlueTails = 0;
            var actualRedTails = 0;
            var actualObstacles = 0;
            var actualEmpty = 0;
            foreach (var field in tronBoard)
            {
                switch (field)
                {
                    case TronBoardField.Empty:
                        actualEmpty++;
                        break;
                    case TronBoardField.Obstacle:
                        actualObstacles++;
                        break;
                    case TronBoardField.BlueTail:
                        actualBlueTails++;
                        break;
                    case TronBoardField.BlueHead:
                        actualBlueHead++;
                        break;
                    case TronBoardField.RedTail:
                        actualRedTails++;
                        break;
                    case TronBoardField.RedHead:
                        actualRedHead++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Assert
            Assert.That(tronBoard.Count(), Is.EqualTo(10 * 20));
            Assert.That(actualBlueHead, Is.EqualTo(expectedBlueHead));
            Assert.That(actualRedHead, Is.EqualTo(expectedRedHead));
            Assert.That(actualBlueTails, Is.EqualTo(expectedBlueTails));
            Assert.That(actualRedTails, Is.EqualTo(expectedRedTails));
            Assert.That(actualObstacles, Is.EqualTo(expectedObstacles));
            Assert.That(actualEmpty, Is.EqualTo(expectedEmpty));
        }

        private static TronBoardField GetRandomField()
        {
            return TestContext.CurrentContext.Random.NextEnum<TronBoardField>();
        }
    }
}