using NSubstitute;
using NUnit.Framework;
using TronBotFramework.Interface.Version1;

namespace TronBotFramework.UnitTests.Interface.Version1
{
    [TestFixture]
    public class TronBotInterfaceTests
    {
        private IBot _bot = null!;
        private IInterface _interface = null!;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _bot = Substitute.For<IBot>();
            _interface = new TronBotInterface();
        }

        [Test]
        public void InterfaceThrowsException_WhenFirstCommandIsNot_Tbi()
        {
            // Arrange
            // Act
            // Assert
            Assert.That(() => _interface.ProcessCommand("not_tbi", _bot), Throws.ArgumentException);
        }

        [Test]
        public void InterfaceThrowsException_WhenSecondCommandIsNot_TbiV1()
        {
            // Arrange
            _interface.ProcessCommand("tbi", _bot);

            // Act
            // Assert
            Assert.That(() => _interface.ProcessCommand("not_tbi_v1", _bot), Throws.ArgumentException);
        }

        [Test]
        public void InterfaceThrowsException_WhenThirdCommandIsNeither_ColorBlue_Nor_ColorRed()
        {
            // Arrange
            _interface.ProcessCommand("tbi", _bot);
            _interface.ProcessCommand("tbi v1", _bot);

            // Act
            // Assert
            Assert.That(() => _interface.ProcessCommand("not_color_blue", _bot), Throws.ArgumentException);
        }

        [Test]
        public void InterfaceInitializationHappyPath()
        {
            // Act
            // Assert
            var response = _interface.ProcessCommand("tbi", _bot);
            Assert.That(response.Content, Is.EqualTo("tbi ok"));
            Assert.That(response.WaitForNextCommand, Is.True);

            response = _interface.ProcessCommand("tbi v1", _bot);
            Assert.That(response.Content, Is.EqualTo("tbi v1 ok"));
            Assert.That(response.WaitForNextCommand, Is.True);

            response = _interface.ProcessCommand("color blue", _bot);
            Assert.That(response.Content, Is.EqualTo("color ok"));
            Assert.That(response.WaitForNextCommand, Is.True);
        }

        [Test]
        public void InterfaceSupportsExitCommand()
        {
            // Arrange
            _interface.ProcessCommand("tbi", _bot);
            _interface.ProcessCommand("tbi v1", _bot);
            _interface.ProcessCommand("color blue", _bot);

            // Act
            var response = _interface.ProcessCommand("exit", _bot);

            // Assert
            Assert.That(response.Content, Is.Empty);
            Assert.That(response.WaitForNextCommand, Is.False);
        }

        [TestCase(Move.Up, "up", "color blue", Color.Blue)]
        [TestCase(Move.Down, "down", "color red", Color.Red)]
        [TestCase(Move.Left, "left", "color blue", Color.Blue)]
        [TestCase(Move.Right, "right", "color red", Color.Red)]
        public void InterfaceSupportsMoveCommand(Move botMove, string responseContent, string colorCommand, Color botColor)
        {
            // Arrange
            _bot.FindMove(Arg.Any<Board>(), Arg.Any<Color>()).Returns(botMove);

            _interface.ProcessCommand("tbi", _bot);
            _interface.ProcessCommand("tbi v1", _bot);
            _interface.ProcessCommand(colorCommand, _bot);

            // Act
            var response = _interface.ProcessCommand("move oooooooooooo/oB9o/o10o/o10o/o10o/o10o/o10o/o10o/o10o/o10o/o9Ro/oooooooooooo", _bot);

            // Assert
            Assert.That(response.Content, Is.EqualTo(responseContent));
            Assert.That(response.WaitForNextCommand, Is.True);
            _bot.Received(1).FindMove(Arg.Is<Board>(b => b != null && b.Width == 12 && b.Height == 12), botColor);
        }
    }
}