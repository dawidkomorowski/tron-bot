using System;
using System.Linq;

namespace TronBotFramework.Interface.Version1
{
    public sealed class TronBotInterface : IInterface
    {
        private State _currentState = State.WaitingForFirstCommand;
        private Color _color;

        public Response ProcessCommand(string command, IBot bot)
        {
            return _currentState switch
            {
                State.WaitingForFirstCommand => HandleCommandInState_WaitingForFirstCommand(command),
                State.TronBotInterfaceAccepted => HandleCommandInState_TronBotInterfaceAccepted(command),
                State.InterfaceVersionAccepted => HandleCommandInState_InterfaceVersionAccepted(command),
                State.Ready => HandleCommandInState_Ready(command, bot),
                _ => throw new ArgumentOutOfRangeException(nameof(_currentState), _currentState, "Unhandled state!")
            };
        }

        private enum State
        {
            WaitingForFirstCommand,
            TronBotInterfaceAccepted,
            InterfaceVersionAccepted,
            Ready
        }

        private Response HandleCommandInState_WaitingForFirstCommand(string command)
        {
            if (command != "tbi") throw new ArgumentException($"Expected 'tbi' command, received '{command}' command.", nameof(command));

            _currentState = State.TronBotInterfaceAccepted;
            return new Response("tbi ok", true);
        }

        private Response HandleCommandInState_TronBotInterfaceAccepted(string command)
        {
            if (command != "tbi v1") throw new ArgumentException($"Expected 'tbi v1' command, received '{command}' command.", nameof(command));

            _currentState = State.InterfaceVersionAccepted;
            return new Response("tbi v1 ok", true);
        }

        private Response HandleCommandInState_InterfaceVersionAccepted(string command)
        {
            if (command != "color blue" && command != "color red")
                throw new ArgumentException($"Expected either 'color blue' command or 'color red' command, received '{command}' command.", nameof(command));

            var colorString = ExtractColorString(command);
            _color = ParseColor(colorString);
            _currentState = State.Ready;
            return new Response("color ok", true);
        }

        private Response HandleCommandInState_Ready(string command, IBot bot)
        {
            if (command == "exit")
            {
                return new Response(string.Empty, false);
            }
            else if (IsMoveCommand(command))
            {
                var rawTronString = ExtractTronString(command);
                var tronString = new TronString(rawTronString);
                var move = bot.FindMove(tronString.Create(), _color);
                return new Response(MapMoveResponse(move), true);
            }
            else
            {
                throw new ArgumentException($"Unrecognized command: {command}.", nameof(command));
            }
        }

        private static bool IsMoveCommand(string command)
        {
            return command.StartsWith("move");
        }

        private static string ExtractTronString(string moveCommand)
        {
            return new string(moveCommand.Skip(5).ToArray());
        }

        private static string MapMoveResponse(Move move) => move switch
        {
            Move.Up => "up",
            Move.Down => "down",
            Move.Left => "left",
            Move.Right => "right",
            _ => throw new ArgumentOutOfRangeException(nameof(move), move, "Unhandled move!")
        };

        private static string ExtractColorString(string colorCommand)
        {
            return new string(colorCommand.Skip(6).ToArray());
        }

        private static Color ParseColor(string color) => color switch
        {
            "blue" => Color.Blue,
            "red" => Color.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, "Unrecognized color!")
        };
    }
}