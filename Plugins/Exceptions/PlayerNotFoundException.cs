using System;

namespace RFRocketLibrary.Plugins.Exceptions
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public class PlayerNotFoundException : Exception
    {
        public override string Message { get; }

        public PlayerNotFoundException(string handle)
        {
            Message = $"Failed to find player by Name/ID '{handle}'";
        }
    }
}