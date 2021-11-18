using System;

namespace RFRocketLibrary.Plugins.Exceptions
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public sealed class ArgumentMissingException : Exception
    {
        public override string Message { get; }

        public ArgumentMissingException(string? name)
        {
            Message = $"Missing Argument {name}";
        }

        public ArgumentMissingException(int index)
        {
            Message = $"Missing Argument at position {index + 1}";
        }
    }
}