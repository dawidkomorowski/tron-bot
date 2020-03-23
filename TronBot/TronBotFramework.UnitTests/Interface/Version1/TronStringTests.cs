using NUnit.Framework;
using TronBotFramework.Interface.Version1;

namespace TronBotFramework.UnitTests.Interface.Version1
{
    [TestFixture]
    public class TronStringTests
    {
        [TestCase("oooooX/orrr1o/o2R1o/o1Bbbo/o3bo/oooooo", "Unrecognized symbol 'X' at position 5.")]
        [TestCase("oooooo/orrr1o/o2R1oo/o1Bbbo/o3bo/oooooo", "Number of fields for row 2 exceeded. Field over limit defined by symbol at 19.")]
        [TestCase("oooooo/orrr1o/o2R1o/o12Bbbo/o3bo/oooooo", "Number of fields for row 3 exceeded. Empty fields over limit defined by symbol at 21.")]
        [TestCase("oooooo/orrr1o/o2R1/o1Bbbo/o3bo/oooooo", "There are missing fields for row 2. Row unexpected ending defined by symbol at 18.")]
        [TestCase("oooooo/orrr1o/o2R1o/o1Bbbo/o3bo/ooooo", "There are missing fields for row 5. Tron string unexpectedly ended.")]
        [TestCase("oooooo/orrr1o/o2R1o/o1Bbbo/o3bo/", "There are missing fields for row 5. Tron string unexpectedly ended.")]
        public void Constructor_ShouldThrowExceptionGivenInvalidTronString(string rawTronString, string expectedMessage)
        {
            // Arrange
            // Act
            // Assert
            Assert.That(() => new TronString(rawTronString), Throws.TypeOf<TronStringParsingException>().With.Message.Contains(expectedMessage));
        }

        [Test]
        public void Create_ShouldCreateBoardBasedOnTronString()
        {
            // Arrange
            const string rawTronString = "oooooo/orrr1o/o2R1o/o1Bbbo/o3bo/oooooo";

            // Act
            var parsedTronString = new TronString(rawTronString);

            // Assert
            var board = parsedTronString.Create();
            Assert.That(board.Width, Is.EqualTo(6));
            Assert.That(board.Height, Is.EqualTo(6));

            Assert.That(board.GetField(0, 0), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(1, 0), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(2, 0), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(3, 0), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(4, 0), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(5, 0), Is.EqualTo(Field.Obstacle));

            Assert.That(board.GetField(0, 1), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(1, 1), Is.EqualTo(Field.RedTail));
            Assert.That(board.GetField(2, 1), Is.EqualTo(Field.RedTail));
            Assert.That(board.GetField(3, 1), Is.EqualTo(Field.RedTail));
            Assert.That(board.GetField(4, 1), Is.EqualTo(Field.Empty));
            Assert.That(board.GetField(5, 1), Is.EqualTo(Field.Obstacle));

            Assert.That(board.GetField(0, 2), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(1, 2), Is.EqualTo(Field.Empty));
            Assert.That(board.GetField(2, 2), Is.EqualTo(Field.Empty));
            Assert.That(board.GetField(3, 2), Is.EqualTo(Field.RedHead));
            Assert.That(board.GetField(4, 2), Is.EqualTo(Field.Empty));
            Assert.That(board.GetField(5, 2), Is.EqualTo(Field.Obstacle));

            Assert.That(board.GetField(0, 3), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(1, 3), Is.EqualTo(Field.Empty));
            Assert.That(board.GetField(2, 3), Is.EqualTo(Field.BlueHead));
            Assert.That(board.GetField(3, 3), Is.EqualTo(Field.BlueTail));
            Assert.That(board.GetField(4, 3), Is.EqualTo(Field.BlueTail));
            Assert.That(board.GetField(5, 3), Is.EqualTo(Field.Obstacle));

            Assert.That(board.GetField(0, 4), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(1, 4), Is.EqualTo(Field.Empty));
            Assert.That(board.GetField(2, 4), Is.EqualTo(Field.Empty));
            Assert.That(board.GetField(3, 4), Is.EqualTo(Field.Empty));
            Assert.That(board.GetField(4, 4), Is.EqualTo(Field.BlueTail));
            Assert.That(board.GetField(5, 4), Is.EqualTo(Field.Obstacle));

            Assert.That(board.GetField(0, 5), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(1, 5), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(2, 5), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(3, 5), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(4, 5), Is.EqualTo(Field.Obstacle));
            Assert.That(board.GetField(5, 5), Is.EqualTo(Field.Obstacle));
        }

        [Test]
        public void Create_ShouldCreateBoard_GivenTronStringWithMultiDigitNumbers()
        {
            // Arrange
            const string rawTronString = "o27o/o26oo";

            // Act
            var parsedTronString = new TronString(rawTronString);

            // Assert
            var board = parsedTronString.Create();
            Assert.That(board.Width, Is.EqualTo(29));
            Assert.That(board.Height, Is.EqualTo(2));
        }
    }
}