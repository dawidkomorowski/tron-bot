using System;
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

        private static TronBoardField GetRandomField()
        {
            return TestContext.CurrentContext.Random.NextEnum<TronBoardField>();
        }
    }
}